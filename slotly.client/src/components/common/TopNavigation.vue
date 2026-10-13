<template>
  <header class="sticky top-0 z-50 w-full border-b border-slate-800/80 bg-[#080c14]/90 backdrop-blur-xl">
    <!-- Top Micro Announcement / Multi-Tenant Context Banner -->
    <div class="announcement-banner px-4 py-1.5 bg-gradient-to-r from-indigo-950/60 via-purple-950/40 to-slate-900 border-b border-indigo-500/10 flex items-center justify-between text-xs text-slate-300">
      <div class="flex items-center gap-2 max-w-7xl mx-auto w-full px-2">
        <span class="inline-flex items-center gap-1.5 px-2 py-0.5 rounded-full bg-indigo-500/20 text-indigo-300 font-medium text-[11px] border border-indigo-500/30">
          <span class="w-1.5 h-1.5 rounded-full bg-indigo-400 animate-pulse"></span>
          Multi-Tenant Engine
        </span>
        <span class="text-slate-400 hidden sm:inline">Active Tenant:</span>
        <span class="font-semibold text-white flex items-center gap-1">
          <Icon :name="currentTenant.logoIcon || 'building'" size="13" class="text-indigo-400" />
          {{ currentTenant.name }}
        </span>
        <span class="text-slate-500 font-mono text-[11px] hidden md:inline">({{ currentTenant.subdomain }})</span>

        <!-- Right Side: Dark/Light Mode & Tenant Switcher -->
        <div class="ml-auto flex items-center gap-3">
          <!-- Light / Dark Mode Toggle Button -->
          <button
            @click="toggleDarkMode"
            class="flex items-center gap-1.5 px-2.5 py-1 rounded-md text-xs font-medium bg-slate-800/80 hover:bg-slate-700 text-slate-200 border border-slate-700/60 transition-colors"
            :title="isDarkMode ? 'Switch to Light Mode' : 'Switch to Dark Mode'"
          >
            <Icon :name="isDarkMode ? 'sun' : 'moon'" size="13" :class="isDarkMode ? 'text-amber-400' : 'text-indigo-400'" />
            <span class="hidden sm:inline">{{ isDarkMode ? 'Light Mode' : 'Dark Mode' }}</span>
          </button>

          <!-- Quick Tenant Switcher Dropdown -->
          <div class="relative">
            <button 
              @click="tenantDropdownOpen = !tenantDropdownOpen"
              class="flex items-center gap-1.5 bg-slate-800/80 hover:bg-slate-700/80 border border-slate-700/60 px-2.5 py-1 rounded-md text-xs font-medium text-slate-200 transition-colors"
            >
              <span class="w-2 h-2 rounded-full" :style="{ backgroundColor: currentTenant.accentColor }"></span>
              <span>{{ currentTenant.name.split(' ')[0] }}</span>
              <Icon name="chevron-down" size="12" class="text-slate-400" />
            </button>

            <!-- Dropdown Menu -->
            <div 
              v-if="tenantDropdownOpen"
              class="absolute right-0 mt-1.5 w-72 rounded-xl bg-slate-900/95 border border-slate-800 shadow-2xl backdrop-blur-xl p-1.5 z-50 text-slate-200"
            >
              <div class="px-2.5 py-1.5 text-[10px] uppercase font-bold tracking-wider text-slate-400 border-b border-slate-800/80">
                Select Active Organization / Venue
              </div>
              <button
                v-for="tenant in tenants"
                :key="tenant.id"
                @click="selectTenant(tenant.id)"
                class="w-full text-left px-2.5 py-2 rounded-lg flex items-center gap-2.5 text-xs hover:bg-slate-800 transition-colors"
                :class="tenant.id === currentTenantId ? 'bg-indigo-600/20 text-indigo-400 font-semibold' : 'text-slate-300'"
              >
                <div 
                  class="w-7 h-7 rounded-lg flex items-center justify-center text-white text-xs font-bold"
                  :style="{ backgroundColor: tenant.accentColor }"
                >
                  <Icon :name="tenant.logoIcon || 'building'" size="14" />
                </div>
                <div class="flex-1 min-w-0">
                  <div class="truncate font-medium">{{ tenant.name }}</div>
                  <div class="text-[10px] text-slate-400 font-mono">{{ tenant.subdomain }}</div>
                </div>
                <Icon v-if="tenant.id === currentTenantId" name="check" size="14" class="text-indigo-400" />
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- Main Navigation Bar -->
    <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8 h-16 flex items-center justify-between gap-4">
      <!-- Brand Logo -->
      <div class="flex items-center gap-3 cursor-pointer select-none" @click="setView('landing')">
        <div class="w-10 h-10 rounded-xl bg-gradient-to-tr from-indigo-600 via-indigo-500 to-purple-500 flex items-center justify-center shadow-lg shadow-indigo-500/25 border border-indigo-400/30">
          <Icon name="calendar" size="22" class="text-white" />
        </div>
        <div>
          <div class="flex items-center gap-1.5">
            <span class="text-xl font-bold tracking-tight text-white">Slotly</span>
            <span class="text-[10px] font-bold px-1.5 py-0.5 rounded bg-indigo-500/20 text-indigo-300 border border-indigo-500/30 uppercase tracking-wide">Cloud</span>
          </div>
          <span class="text-[11px] text-slate-400 block -mt-0.5">Booking Engine for Services, Hotels & Venues</span>
        </div>
      </div>

      <!-- Navigation Mode Switcher Pills -->
      <nav class="hidden md:flex items-center p-1 rounded-xl bg-slate-900/90 border border-slate-800/80 shadow-inner">
        <button
          @click="setView('landing')"
          class="flex items-center gap-2 px-3.5 py-1.5 rounded-lg text-xs font-medium transition-all duration-200"
          :class="currentView === 'landing' ? 'bg-indigo-600 text-white shadow-md shadow-indigo-600/30' : 'text-slate-400 hover:text-white hover:bg-slate-800/50'"
        >
          <Icon name="globe" size="14" />
          <span>SaaS Platform</span>
        </button>

        <button
          @click="setView('client-portal')"
          class="flex items-center gap-2 px-3.5 py-1.5 rounded-lg text-xs font-medium transition-all duration-200"
          :class="currentView === 'client-portal' ? 'bg-indigo-600 text-white shadow-md shadow-indigo-600/30' : 'text-slate-400 hover:text-white hover:bg-slate-800/50'"
        >
          <Icon name="calendar" size="14" />
          <span>Client Booking Flow</span>
          <span class="w-1.5 h-1.5 rounded-full bg-emerald-400"></span>
        </button>

        <button
          @click="setView('tenant-dashboard')"
          class="flex items-center gap-2 px-3.5 py-1.5 rounded-lg text-xs font-medium transition-all duration-200"
          :class="currentView === 'tenant-dashboard' ? 'bg-indigo-600 text-white shadow-md shadow-indigo-600/30' : 'text-slate-400 hover:text-white hover:bg-slate-800/50'"
        >
          <Icon name="bar-chart" size="14" />
          <span>Tenant Admin Portal</span>
          <span class="px-1.5 py-0.2 rounded text-[10px] bg-slate-800 text-slate-300 font-mono">{{ currentTenant.bookings.length }}</span>
        </button>
      </nav>

      <!-- Action Button Group with User Authentication & Dashboard Link -->
      <div class="flex items-center gap-3">
        <!-- User Auth Badge / Login Action -->
        <div v-if="currentUser" class="flex items-center gap-2 bg-slate-900/80 border border-slate-800 rounded-xl px-2.5 py-1.5 text-xs">
          <div class="w-6 h-6 rounded-full bg-indigo-600 text-white font-bold flex items-center justify-center text-[10px]">
            {{ currentUser.name.charAt(0).toUpperCase() }}
          </div>
          <span class="text-white font-medium max-w-[90px] truncate hidden sm:inline">{{ currentUser.name }}</span>
          <button 
            @click="logoutUser" 
            title="Sign Out"
            class="text-slate-400 hover:text-rose-400 transition-colors p-0.5"
          >
            <Icon name="log-out" size="13" />
          </button>
        </div>

        <div v-else class="flex items-center gap-1.5">
          <button
            @click="openAuth('client', 'login')"
            class="px-3 py-1.5 rounded-lg text-xs font-medium text-slate-300 hover:text-white hover:bg-slate-800 border border-slate-700/60 transition-colors"
          >
            Sign In
          </button>
          <button
            @click="openAuth('tenant', 'register')"
            class="hidden sm:inline-flex px-3 py-1.5 rounded-lg text-xs font-medium text-indigo-300 bg-indigo-950/40 hover:bg-indigo-900/40 border border-indigo-500/30 transition-colors"
          >
            Register Org
          </button>
        </div>

        <button
          v-if="currentView !== 'tenant-dashboard'"
          @click="setView('tenant-dashboard')"
          class="inline-flex items-center gap-2 px-3.5 py-2 rounded-lg text-xs font-semibold text-white bg-gradient-to-r from-indigo-600 to-purple-600 hover:from-indigo-500 hover:to-purple-500 shadow-md shadow-indigo-600/25 transition-all transform hover:-translate-y-0.5"
        >
          <Icon name="sliders" size="14" />
          <span class="hidden sm:inline">Admin Dashboard</span>
        </button>

        <button
          v-else
          @click="setView('client-portal')"
          class="inline-flex items-center gap-2 px-3.5 py-2 rounded-lg text-xs font-semibold text-white bg-gradient-to-r from-emerald-600 to-teal-600 hover:from-emerald-500 hover:to-teal-500 shadow-md shadow-emerald-600/25 transition-all"
        >
          <Icon name="calendar" size="14" />
          <span class="hidden sm:inline">Booking Widget</span>
        </button>
      </div>
    </div>

    <!-- Mobile Navigation Sub-bar -->
    <div class="md:hidden px-4 py-2 bg-slate-900/90 border-t border-slate-800 flex items-center justify-around text-xs">
      <button 
        @click="setView('landing')" 
        class="py-1 px-2 rounded font-medium"
        :class="currentView === 'landing' ? 'text-indigo-400 font-semibold' : 'text-slate-400'"
      >
        Platform
      </button>
      <button 
        @click="setView('client-portal')" 
        class="py-1 px-2 rounded font-medium"
        :class="currentView === 'client-portal' ? 'text-indigo-400 font-semibold' : 'text-slate-400'"
      >
        Book Service
      </button>
      <button 
        @click="setView('tenant-dashboard')" 
        class="py-1 px-2 rounded font-medium"
        :class="currentView === 'tenant-dashboard' ? 'text-indigo-400 font-semibold' : 'text-slate-400'"
      >
        Dashboard
      </button>
    </div>
  </header>
</template>

<script setup>
import { ref } from 'vue'
import Icon from './Icon.vue'
import { useSlotly } from '../../composables/useSlotly'

const {
  tenants,
  currentTenantId,
  currentTenant,
  currentView,
  isDarkMode,
  toggleDarkMode,
  currentUser,
  openAuth,
  logoutUser,
  setView,
  switchTenant
} = useSlotly()

const tenantDropdownOpen = ref(false)

function selectTenant(id) {
  switchTenant(id)
  tenantDropdownOpen.value = false
}
</script>
