<template>
  <div class="min-h-screen pb-20 pt-6 px-4 sm:px-6 lg:px-8 max-w-7xl mx-auto">
    <!-- Dashboard Header Banner -->
    <div class="glass-card rounded-2xl p-6 mb-6 flex flex-col md:flex-row md:items-center justify-between gap-6 border border-slate-800 shadow-2xl relative overflow-hidden">
      <!-- Glow effect -->
      <div 
        class="absolute -left-20 -top-20 w-72 h-72 rounded-full blur-3xl opacity-15 pointer-events-none"
        :style="{ backgroundColor: currentTenant.accentColor }"
      ></div>

      <div class="relative z-10 flex items-center gap-4">
        <div 
          class="w-14 h-14 rounded-2xl flex items-center justify-center text-white shadow-xl flex-shrink-0"
          :style="{ backgroundColor: currentTenant.accentColor }"
        >
          <Icon :name="currentTenant.logoIcon || 'building'" size="28" class="text-white" />
        </div>
        <div>
          <div class="flex items-center gap-2">
            <h1 class="text-xl sm:text-2xl font-extrabold text-white tracking-tight">{{ currentTenant.name }}</h1>
            <span class="px-2 py-0.5 rounded text-[10px] font-bold bg-indigo-500/20 text-indigo-300 border border-indigo-500/30 uppercase">
              Admin Portal
            </span>
          </div>
          <div class="flex items-center gap-3 text-xs text-slate-400 mt-1">
            <span class="font-mono text-indigo-300">{{ currentTenant.subdomain }}</span>
            <span>•</span>
            <span class="text-emerald-400 font-medium">Auto-Sync Live</span>
            <span>•</span>
            <span>{{ currentBookings.length }} Active Appointments</span>
          </div>
        </div>
      </div>

      <div class="relative z-10 flex items-center flex-wrap gap-2.5">
        <button
          @click="isNewBookingOpen = true"
          class="px-4 py-2 rounded-xl text-xs font-semibold text-white bg-indigo-600 hover:bg-indigo-500 shadow-lg shadow-indigo-600/30 flex items-center gap-1.5 transition-all transform hover:-translate-y-0.5"
        >
          <Icon name="plus" size="14" />
          <span>New Appointment</span>
        </button>

        <button
          @click="setView('client-portal')"
          class="px-3.5 py-2 rounded-xl text-xs font-medium text-slate-200 bg-slate-800 hover:bg-slate-700 border border-slate-700/80 flex items-center gap-1.5 transition-colors"
        >
          <Icon name="external-link" size="14" />
          <span>View Public Booking</span>
        </button>
      </div>
    </div>

    <!-- Tab Navigation Bar -->
    <div class="flex items-center gap-1.5 overflow-x-auto pb-2 mb-6 border-b border-slate-800 text-xs font-medium">
      <button
        v-for="tab in tabs"
        :key="tab.id"
        @click="setDashboardTab(tab.id)"
        class="flex items-center gap-2 px-4 py-2.5 rounded-xl transition-all whitespace-nowrap"
        :class="activeDashboardTab === tab.id ? 'bg-indigo-600/20 text-white font-semibold border border-indigo-500/40 shadow-sm' : 'text-slate-400 hover:text-white hover:bg-slate-800/40'"
      >
        <Icon :name="tab.icon" size="15" :class="activeDashboardTab === tab.id ? 'text-indigo-400' : 'text-slate-500'" />
        <span>{{ tab.label }}</span>
        <span 
          v-if="tab.badge"
          class="px-1.5 py-0.2 rounded-full text-[10px] bg-slate-800 text-slate-300 font-mono"
        >
          {{ tab.badge }}
        </span>
      </button>
    </div>

    <!-- Active Tab Content -->
    <div class="transition-all duration-200">
      <DashboardOverview
        v-if="activeDashboardTab === 'overview'"
        @switchTab="setDashboardTab"
        @selectBooking="openBookingDetail"
      />

      <CalendarView
        v-else-if="activeDashboardTab === 'calendar'"
        @selectBooking="openBookingDetail"
        @openNewBooking="isNewBookingOpen = true"
      />

      <BookingsTable
        v-else-if="activeDashboardTab === 'bookings'"
        @selectBooking="openBookingDetail"
        @openNewBooking="isNewBookingOpen = true"
      />

      <ServicesManager
        v-else-if="activeDashboardTab === 'services'"
      />

      <StaffManager
        v-else-if="activeDashboardTab === 'staff'"
      />

      <TenantSettings
        v-else-if="activeDashboardTab === 'settings'"
      />
    </div>

    <!-- Modals -->
    <NewBookingModal
      :isOpen="isNewBookingOpen"
      @close="isNewBookingOpen = false"
    />

    <BookingDetailModal
      :booking="selectedBooking"
      @close="selectedBooking = null"
    />
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import Icon from '../common/Icon.vue'
import DashboardOverview from './DashboardOverview.vue'
import CalendarView from './CalendarView.vue'
import BookingsTable from './BookingsTable.vue'
import ServicesManager from './ServicesManager.vue'
import StaffManager from './StaffManager.vue'
import TenantSettings from './TenantSettings.vue'
import NewBookingModal from '../modals/NewBookingModal.vue'
import BookingDetailModal from '../modals/BookingDetailModal.vue'
import { useSlotly } from '../../composables/useSlotly'

const {
  currentTenant,
  currentBookings,
  currentServices,
  currentStaff,
  activeDashboardTab,
  setDashboardTab,
  setView
} = useSlotly()

const isNewBookingOpen = ref(false)
const selectedBooking = ref(null)

const tabs = computed(() => [
  { id: 'overview', label: 'Overview', icon: 'bar-chart' },
  { id: 'calendar', label: 'Schedule Calendar', icon: 'calendar', badge: currentBookings.value.length },
  { id: 'bookings', label: 'All Bookings', icon: 'users', badge: currentBookings.value.length },
  { id: 'services', label: 'Services & Pricing', icon: 'sparkles', badge: currentServices.value.length },
  { id: 'staff', label: 'Specialists & Staff', icon: 'user', badge: currentStaff.value.length },
  { id: 'settings', label: 'Settings & White-Label', icon: 'settings' }
])

function openBookingDetail(booking) {
  selectedBooking.value = booking
}
</script>
