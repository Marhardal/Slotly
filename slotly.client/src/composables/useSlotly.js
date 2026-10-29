import { ref, reactive, computed } from 'vue'
import { initialTenants, promoCodes } from '../data/mockData'
import { slotlyApi } from '../services/api'

const backendTenantIds = {
  'tenant-aura': 'a1000000-0000-4000-8000-000000000001',
  'tenant-luminary': 'a1000000-0000-4000-8000-000000000002',
  'tenant-apex': 'a1000000-0000-4000-8000-000000000003',
  'tenant-nexcode': 'a1000000-0000-4000-8000-000000000004'
}

// Global shared state
const tenants = reactive(JSON.parse(JSON.stringify(initialTenants)))
const backendData = reactive({
  categories: [],
  statuses: [],
  tenants: {},
  loading: {}
})
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
let referenceDataRequest
let referenceDataLoaded = false

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

function loadReferenceData() {
  if (referenceDataLoaded) {
    return Promise.resolve()
  }

  if (!referenceDataRequest) {
    referenceDataRequest = Promise.all([
      slotlyApi.getCategories(),
      slotlyApi.getStatuses()
    ]).then(([categories, statuses]) => {
      backendData.categories = categories
      backendData.statuses = statuses
      referenceDataLoaded = true
    }).finally(() => {
      referenceDataRequest = null
    })
  }

  return referenceDataRequest
}

function mapService(service) {
  const category = backendData.categories.find(item => item.id === service.categoryID)
  return {
    id: service.id,
    categoryID: service.categoryID,
    name: service.name,
    category: category?.name || 'Uncategorized',
    durationMinutes: service.duration,
    price: service.price,
    description: service.description || '',
    popular: service.popular,
    icon: 'sparkles'
  }
}

function formatTime(time) {
  if (!time) return ''
  const [hours, minutes] = time.split(':').map(Number)
  const suffix = hours >= 12 ? 'PM' : 'AM'
  return `${hours % 12 || 12}:${String(minutes).padStart(2, '0')} ${suffix}`
}

function mapBooking(booking, services) {
  const service = services.find(item => item.id === booking.serviceID)
  const status = backendData.statuses.find(item => item.id === booking.statusID)
  const addons = (booking.addOns || []).map(addon => ({
    id: addon.id,
    name: addon.name,
    price: addon.price
  }))
  const addonsTotal = addons.reduce((total, addon) => total + addon.price, 0)
  const basePrice = Math.max(0, booking.price - addonsTotal)
  return {
    id: booking.id,
    clientName: booking.clientName || 'Client',
    clientEmail: booking.clientEmail || '',
    clientPhone: booking.clientPhone || '',
    serviceId: booking.serviceID,
    serviceName: service?.name || 'Service unavailable',
    staffId: booking.staffID || '',
    staffName: booking.staffName || 'Unassigned',
    date: booking.date,
    time: formatTime(booking.time),
    duration: booking.duration,
    price: booking.price,
    status: (status?.name || 'Pending').toLowerCase(),
    notes: booking.notes || '',
    createdAt: booking.createdOn,
    pricing: {
      basePrice,
      addons,
      addonsTotal,
      grandTotal: booking.price,
      paymentOption: 'not_processed',
      paymentMethod: 'Not collected'
    },
    statusID: booking.statusID
  }
}

async function loadTenantData(tenantId = currentTenantId.value) {
  if (backendData.tenants[tenantId]) return backendData.tenants[tenantId]
  if (backendData.loading[tenantId]) return backendData.loading[tenantId]

  const backendTenantId = backendTenantIds[tenantId]
  if (!backendTenantId) {
    throw new Error(`No backend tenant mapping exists for '${tenantId}'.`)
  }

  backendData.loading[tenantId] = (async () => {
    await loadReferenceData()
    const [services, addons, bookings] = await Promise.all([
      slotlyApi.getServices(backendTenantId),
      slotlyApi.getAddons(backendTenantId),
      slotlyApi.getBookings(backendTenantId)
    ])
    const mappedServices = services.map(mapService)
    const data = {
      services: mappedServices,
      addons: addons.map(addon => ({
        id: addon.id,
        serviceId: addon.serviceID,
        name: addon.name,
        description: addon.description || '',
        price: addon.price,
        durationMinutes: addon.duration
      })),
      bookings: bookings.map(booking => mapBooking(booking, mappedServices))
    }
    backendData.tenants[tenantId] = data
    return data
  })()

  try {
    return await backendData.loading[tenantId]
  } finally {
    delete backendData.loading[tenantId]
  }
}

// Initialize theme once on script load
if (typeof window !== 'undefined') {
  initTheme()
  loadTenantData().catch(error => {
    showToast('Backend Connection Failed', error.message, 'error')
  })
}

export function useSlotly() {
  const currentTenant = computed(() => {
    return tenants.find(t => t.id === currentTenantId.value) || tenants[0]
  })

  const currentBookings = computed(() => {
    return backendData.tenants[currentTenantId.value]?.bookings ??
      (currentTenant.value ? currentTenant.value.bookings : [])
  })

  const currentServices = computed(() => {
    return backendData.tenants[currentTenantId.value]?.services ??
      (currentTenant.value ? currentTenant.value.services : [])
  })

  const currentStaff = computed(() => {
    return currentTenant.value ? currentTenant.value.staff : []
  })

  const currentAddons = computed(() => {
    return backendData.tenants[currentTenantId.value]?.addons ??
      (currentTenant.value ? (currentTenant.value.addons || []) : [])
  })

  const currentCategories = computed(() => backendData.categories)
  const isBackendLoaded = computed(() => Boolean(backendData.tenants[currentTenantId.value]))

  // Dynamic dashboard stats computed from live bookings and transactions
  const computedStats = computed(() => {
    if (!currentTenant.value) return { totalRevenue: 0, totalBookings: 0, confirmed: 0, pending: 0 }
    
    const bks = currentBookings.value
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
      revenue: backendData.tenants[currentTenantId.value]
        ? totalRev
        : totalRev + (currentTenant.value.stats.revenueMonth || 0),
      totalBookings: backendData.tenants[currentTenantId.value]
        ? bks.length
        : bks.length + (currentTenant.value.stats.bookingsMonth || 0),
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
      loadTenantData(tenantId).catch(error => {
        showToast('Backend Connection Failed', error.message, 'error')
      })
    }
  }

  function setView(view) {
    currentView.value = view
    window.scrollTo({ top: 0, behavior: 'smooth' })
  }

  function setDashboardTab(tab) {
    activeDashboardTab.value = tab
  }

  async function createBooking(payload) {
    if (!currentTenant.value) throw new Error('No tenant is selected.')
    await loadTenantData()

    const tenantId = backendTenantIds[currentTenantId.value]
    const timeMatch = payload.time.match(/^(\d{1,2}):(\d{2})\s*(AM|PM)$/i)
    if (!timeMatch) throw new Error('Choose a valid booking time.')
    let hour = Number(timeMatch[1]) % 12
    if (timeMatch[3].toUpperCase() === 'PM') hour += 12

    const created = await slotlyApi.createBooking({
      tenantID: tenantId,
      serviceID: payload.serviceId,
      statusID: '00000000-0000-0000-0000-000000000000',
      date: payload.date,
      time: `${String(hour).padStart(2, '0')}:${timeMatch[2]}:00`,
      clientName: payload.clientName,
      clientEmail: payload.clientEmail,
      clientPhone: payload.clientPhone || '',
      staffID: payload.staffId || '',
      staffName: payload.staffName || '',
      notes: payload.notes || '',
      addOnIDs: (payload.addons || []).map(addon => addon.id)
    })

    const newBooking = mapBooking(created, backendData.tenants[currentTenantId.value].services)
    backendData.tenants[currentTenantId.value].bookings.unshift(newBooking)
    showToast('Booking Request Saved', 'Your booking request is recorded. Payment has not been processed.', 'success')
    return newBooking
  }

  async function updateBookingStatus(bookingId, status) {
    await loadTenantData()
    const tenantData = backendData.tenants[currentTenantId.value]
    const booking = tenantData.bookings.find(item => item.id === bookingId)
    const targetStatus = backendData.statuses.find(item =>
      item.name?.toLowerCase() === status.toLowerCase() && item.category === 1)
    if (!booking || !targetStatus) {
      throw new Error(`Could not find booking '${bookingId}' or status '${status}'.`)
    }

    const tenantId = backendTenantIds[currentTenantId.value]
    const timeMatch = booking.time.match(/^(\d{1,2}):(\d{2})\s*(AM|PM)$/i)
    let hour = Number(timeMatch[1]) % 12
    if (timeMatch[3].toUpperCase() === 'PM') hour += 12
    await slotlyApi.updateBooking({
      id: booking.id,
      tenantID: tenantId,
      serviceID: booking.serviceId,
      statusID: targetStatus.id,
      date: booking.date,
      time: `${String(hour).padStart(2, '0')}:${timeMatch[2]}:00`,
      clientName: booking.clientName,
      clientEmail: booking.clientEmail,
      clientPhone: booking.clientPhone,
      staffID: booking.staffId,
      staffName: booking.staffName,
      duration: booking.duration,
      price: booking.price,
      notes: booking.notes
    })
    booking.status = status.toLowerCase()
    booking.statusID = targetStatus.id
    showToast('Status Updated', `Booking #${bookingId} marked as ${status}.`, 'info')
  }

  async function addService(serviceData) {
    await loadTenantData()
    const created = await slotlyApi.createService({
      tenantID: backendTenantIds[currentTenantId.value],
      categoryID: serviceData.categoryID,
      name: serviceData.name,
      description: serviceData.description || '',
      duration: Number(serviceData.durationMinutes),
      price: Number(serviceData.price),
      popular: Boolean(serviceData.popular)
    })
    const newService = mapService(created)
    backendData.tenants[currentTenantId.value].services.push(newService)
    showToast('Service Created', `Added ${newService.name} to catalog.`, 'success')
    return newService
  }

  async function deleteService(serviceId) {
    await loadTenantData()
    await slotlyApi.deleteService(serviceId, backendTenantIds[currentTenantId.value])
    backendData.tenants[currentTenantId.value].services =
      backendData.tenants[currentTenantId.value].services.filter(service => service.id !== serviceId)
    showToast('Service Removed', 'The service was removed from the catalog.', 'info')
  }

  async function addAddon(addonData) {
    await loadTenantData()
    const created = await slotlyApi.createAddon({
      serviceID: addonData.serviceID,
      name: addonData.name,
      description: addonData.description || '',
      price: Number(addonData.price),
      duration: Number(addonData.durationMinutes) || 0
    })
    const newAddon = {
      id: created.id,
      serviceId: created.serviceID,
      name: created.name,
      description: created.description || '',
      price: created.price,
      durationMinutes: created.duration
    }
    backendData.tenants[currentTenantId.value].addons.push(newAddon)
    showToast('Add-on Created', `Added ${newAddon.name} to the service.`, 'success')
    return newAddon
  }

  async function deleteAddon(addonId) {
    await loadTenantData()
    await slotlyApi.deleteAddon(addonId)
    backendData.tenants[currentTenantId.value].addons =
      backendData.tenants[currentTenantId.value].addons.filter(addon => addon.id !== addonId)
    showToast('Add-on Removed', 'The add-on was removed from the catalog.', 'info')
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
    currentCategories,
    isBackendLoaded,
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
    addAddon,
    deleteAddon,
    addStaffMember,
    updateTenantSettings,
    loadTenantData
  }
}
