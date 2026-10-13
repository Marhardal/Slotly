import { ref, reactive, computed } from 'vue'
import { initialTenants, promoCodes } from '../data/mockData'

// Global shared state
const tenants = reactive(JSON.parse(JSON.stringify(initialTenants)))
const currentTenantId = ref('tenant-aura')
const currentView = ref('landing') // 'landing' | 'client-portal' | 'tenant-dashboard'
const activeDashboardTab = ref('overview') // 'overview' | 'calendar' | 'bookings' | 'services' | 'staff' | 'settings'

// Dark / Light Theme Mode State
const isDarkMode = ref(true)

function initTheme() {
  const saved = localStorage.getItem('slotly_theme')
  if (saved) {
    isDarkMode.value = saved === 'dark'
  } else {
    isDarkMode.value = true
  }
  applyTheme()
}

function applyTheme() {
  if (isDarkMode.value) {
    document.documentElement.classList.add('dark')
    document.documentElement.classList.remove('light')
  } else {
    document.documentElement.classList.add('light')
    document.documentElement.classList.remove('dark')
  }
}

function toggleDarkMode() {
  isDarkMode.value = !isDarkMode.value
  localStorage.setItem('slotly_theme', isDarkMode.value ? 'dark' : 'light')
  applyTheme()
  showToast(
    isDarkMode.value ? 'Dark Mode Active' : 'Light Mode Active',
    `Theme switched to ${isDarkMode.value ? 'midnight dark' : 'crisp light'} ambiance.`,
    'info'
  )
}

// User Authentication State
const currentUser = ref(null) // null or { name, email, role: 'client'|'tenant', tenantId }
const isAuthModalOpen = ref(false)
const authModalTab = ref('client') // 'client' | 'tenant'
const authModalMode = ref('login') // 'login' | 'register'

function openAuth(tab = 'client', mode = 'login') {
  authModalTab.value = tab
  authModalMode.value = mode
  isAuthModalOpen.value = true
}

function closeAuth() {
  isAuthModalOpen.value = false
}

function loginUser(credentials) {
  currentUser.value = {
    name: credentials.email.split('@')[0],
    email: credentials.email,
    role: credentials.role || 'client',
    tenantId: credentials.tenantId || currentTenantId.value
  }
  isAuthModalOpen.value = false
  showToast('Logged In', `Welcome back, ${currentUser.value.name}!`, 'success')

  if (credentials.role === 'tenant') {
    if (credentials.tenantId) {
      currentTenantId.value = credentials.tenantId
    }
    currentView.value = 'tenant-dashboard'
  }
}

function registerUser(userData) {
  currentUser.value = {
    name: userData.name || userData.email.split('@')[0],
    email: userData.email,
    role: userData.role || 'client',
    tenantId: userData.tenantId || currentTenantId.value
  }
  isAuthModalOpen.value = false
  showToast('Account Created', `Account successfully registered for ${currentUser.value.email}.`, 'success')

  if (userData.role === 'tenant') {
    currentView.value = 'tenant-dashboard'
  }
}

function logoutUser() {
  currentUser.value = null
  showToast('Signed Out', 'You have been successfully signed out.', 'info')
}

// Toast notification system
const toasts = ref([])

function showToast(title, message, type = 'success') {
  const id = Date.now() + Math.random()
  toasts.value.push({ id, title, message, type })
  setTimeout(() => {
    toasts.value = toasts.value.filter(t => t.id !== id)
  }, 4500)
}

function removeToast(id) {
  toasts.value = toasts.value.filter(t => t.id !== id)
}

// Initialize theme once on script load
if (typeof window !== 'undefined') {
  initTheme()
}

export function useSlotly() {
  const currentTenant = computed(() => {
    return tenants.find(t => t.id === currentTenantId.value) || tenants[0]
  })

  const currentBookings = computed(() => {
    return currentTenant.value ? currentTenant.value.bookings : []
  })

  const currentServices = computed(() => {
    return currentTenant.value ? currentTenant.value.services : []
  })

  const currentStaff = computed(() => {
    return currentTenant.value ? currentTenant.value.staff : []
  })

  const currentAddons = computed(() => {
    return currentTenant.value ? (currentTenant.value.addons || []) : []
  })

  // Dynamic dashboard stats computed from live bookings and transactions
  const computedStats = computed(() => {
    if (!currentTenant.value) return { totalRevenue: 0, totalBookings: 0, confirmed: 0, pending: 0 }
    
    const bks = currentTenant.value.bookings
    const totalRev = bks.reduce((acc, curr) => {
      if (curr.status !== 'cancelled') {
        const val = curr.pricing?.grandTotal ?? curr.price ?? 0
        return acc + val
      }
      return acc
    }, 0)

    const confirmedCount = bks.filter(b => b.status === 'confirmed').length
    const pendingCount = bks.filter(b => b.status === 'pending').length
    const completedCount = bks.filter(b => b.status === 'completed').length

    return {
      revenue: totalRev + (currentTenant.value.stats.revenueMonth || 0),
      totalBookings: bks.length + (currentTenant.value.stats.bookingsMonth || 0),
      confirmedCount,
      pendingCount,
      completedCount,
      occupancyRate: currentTenant.value.stats.occupancyRate,
      rating: currentTenant.value.stats.rating,
      retentionRate: currentTenant.value.stats.retentionRate
    }
  })

  function switchTenant(tenantId) {
    if (tenants.some(t => t.id === tenantId)) {
      currentTenantId.value = tenantId
      showToast('Switched Tenant', `Now active in ${currentTenant.value.name}`, 'info')
    }
  }

  function setView(view) {
    currentView.value = view
    window.scrollTo({ top: 0, behavior: 'smooth' })
  }

  function setDashboardTab(tab) {
    activeDashboardTab.value = tab
  }

  function createBooking(payload) {
    if (!currentTenant.value) return null

    const transactionId = payload.pricing?.transactionId || `TXN-${Math.floor(10000 + Math.random() * 90000)}`
    const grandTotal = payload.pricing?.grandTotal ?? payload.price ?? 100

    const newBooking = {
      id: `bk-${Math.floor(1000 + Math.random() * 9000)}`,
      clientName: payload.clientName,
      clientEmail: payload.clientEmail,
      clientPhone: payload.clientPhone || '',
      serviceId: payload.serviceId,
      serviceName: payload.serviceName,
      staffId: payload.staffId,
      staffName: payload.staffName,
      date: payload.date,
      time: payload.time,
      duration: payload.duration || 60,
      price: grandTotal,
      pricing: payload.pricing || {
        basePrice: payload.price || 100,
        addons: [],
        addonsTotal: 0,
        discount: 0,
        discountCode: null,
        subtotal: payload.price || 100,
        tax: Number(((payload.price || 100) * 0.085).toFixed(2)),
        processingFee: 2.50,
        tip: 0,
        grandTotal: grandTotal,
        paymentOption: 'full',
        depositAmount: grandTotal,
        balanceDue: 0,
        paymentMethod: 'Credit Card',
        transactionId
      },
      status: payload.status || 'confirmed',
      notes: payload.notes || '',
      createdAt: new Date().toISOString()
    }

    currentTenant.value.bookings.unshift(newBooking)
    showToast(
      'Transaction Processed!',
      `Receipt #${transactionId} generated for ${payload.clientName} ($${Number(grandTotal).toFixed(2)} USD).`,
      'success'
    )
    return newBooking
  }

  function updateBookingStatus(bookingId, status) {
    const booking = currentTenant.value.bookings.find(b => b.id === bookingId)
    if (booking) {
      booking.status = status
      showToast('Status Updated', `Booking #${bookingId} marked as ${status}.`, 'info')
    }
  }

  function addService(serviceData) {
    const newService = {
      id: `srv-${Date.now()}`,
      name: serviceData.name,
      category: serviceData.category || 'General',
      durationMinutes: Number(serviceData.durationMinutes) || 60,
      price: Number(serviceData.price) || 100,
      description: serviceData.description || '',
      popular: Boolean(serviceData.popular),
      icon: 'sparkles'
    }
    currentTenant.value.services.push(newService)
    showToast('Service Created', `Added ${newService.name} to catalog.`, 'success')
    return newService
  }

  function deleteService(serviceId) {
    currentTenant.value.services = currentTenant.value.services.filter(s => s.id !== serviceId)
    showToast('Service Removed', 'The service was removed from the catalog.', 'info')
  }

  function addStaffMember(staffData) {
    const newStaff = {
      id: `stf-${Date.now()}`,
      name: staffData.name,
      role: staffData.role,
      rating: 5.0,
      avatar: staffData.avatar || 'https://images.unsplash.com/photo-1534528741775-53994a69daeb?w=150&auto=format&fit=crop&q=80',
      availableDays: staffData.availableDays || ['Mon', 'Tue', 'Wed', 'Thu', 'Fri'],
      specialties: staffData.specialties || ['General Service']
    }
    currentTenant.value.staff.push(newStaff)
    showToast('Specialist Added', `Welcome ${newStaff.name} to the team!`, 'success')
    return newStaff
  }

  function updateTenantSettings(newSettings) {
    if (!currentTenant.value) return
    Object.assign(currentTenant.value, newSettings)
    showToast('Settings Saved', 'Tenant branding and policies have been updated.', 'success')
  }

  return {
    tenants,
    currentTenantId,
    currentTenant,
    currentBookings,
    currentServices,
    currentStaff,
    currentAddons,
    computedStats,
    currentView,
    activeDashboardTab,
    promoCodes,
    isDarkMode,
    toggleDarkMode,
    currentUser,
    isAuthModalOpen,
    authModalTab,
    authModalMode,
    openAuth,
    closeAuth,
    loginUser,
    registerUser,
    logoutUser,
    toasts,
    showToast,
    removeToast,
    switchTenant,
    setView,
    setDashboardTab,
    createBooking,
    updateBookingStatus,
    addService,
    deleteService,
    addStaffMember,
    updateTenantSettings
  }
}
