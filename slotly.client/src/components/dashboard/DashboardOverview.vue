<template>
  <div class="space-y-6">
    <!-- Top Stat KPI Cards -->
    <div class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-4 gap-4">
      <!-- Revenue Card -->
      <div class="glass-card rounded-2xl p-5 border border-slate-800/80 hover:border-slate-700 transition-all">
        <div class="flex items-center justify-between text-slate-400">
          <span class="text-xs font-medium">Estimated Revenue</span>
          <div class="w-8 h-8 rounded-lg bg-emerald-500/10 text-emerald-400 flex items-center justify-center">
            <Icon name="dollar-sign" size="16" />
          </div>
        </div>
        <div class="text-2xl font-bold text-white mt-2">
          ${{ computedStats.revenue.toLocaleString() }}
        </div>
        <div class="flex items-center gap-1.5 text-xs text-emerald-400 mt-2 font-medium">
          <span>+14.2%</span>
          <span class="text-slate-500">vs last period</span>
        </div>
      </div>

      <!-- Bookings Card -->
      <div class="glass-card rounded-2xl p-5 border border-slate-800/80 hover:border-slate-700 transition-all">
        <div class="flex items-center justify-between text-slate-400">
          <span class="text-xs font-medium">Total Bookings</span>
          <div class="w-8 h-8 rounded-lg bg-indigo-500/10 text-indigo-400 flex items-center justify-center">
            <Icon name="calendar" size="16" />
          </div>
        </div>
        <div class="text-2xl font-bold text-white mt-2">
          {{ computedStats.totalBookings }}
        </div>
        <div class="flex items-center gap-2 text-xs text-slate-400 mt-2">
          <span class="text-emerald-400 font-medium">{{ computedStats.confirmedCount }} confirmed</span>
          <span>•</span>
          <span class="text-amber-400 font-medium">{{ computedStats.pendingCount }} pending</span>
        </div>
      </div>

      <!-- Occupancy Rate Card -->
      <div class="glass-card rounded-2xl p-5 border border-slate-800/80 hover:border-slate-700 transition-all">
        <div class="flex items-center justify-between text-slate-400">
          <span class="text-xs font-medium">Slot Occupancy</span>
          <div class="w-8 h-8 rounded-lg bg-cyan-500/10 text-cyan-400 flex items-center justify-center">
            <Icon name="clock" size="16" />
          </div>
        </div>
        <div class="text-2xl font-bold text-white mt-2">
          {{ computedStats.occupancyRate }}%
        </div>
        <div class="w-full bg-slate-800 h-1.5 rounded-full mt-3 overflow-hidden">
          <div class="bg-cyan-500 h-full rounded-full" :style="{ width: `${computedStats.occupancyRate}%` }"></div>
        </div>
      </div>

      <!-- Rating / Retention Card -->
      <div class="glass-card rounded-2xl p-5 border border-slate-800/80 hover:border-slate-700 transition-all">
        <div class="flex items-center justify-between text-slate-400">
          <span class="text-xs font-medium">Satisfaction Score</span>
          <div class="w-8 h-8 rounded-lg bg-amber-500/10 text-amber-400 flex items-center justify-center">
            <Icon name="star" size="16" />
          </div>
        </div>
        <div class="text-2xl font-bold text-white mt-2">
          {{ computedStats.rating }} / 5.0
        </div>
        <div class="flex items-center gap-1.5 text-xs text-indigo-300 mt-2 font-medium">
          <span>{{ computedStats.retentionRate }}% retention rate</span>
        </div>
      </div>
    </div>

    <!-- Middle Split: Today's Schedule & Quick Actions -->
    <div class="grid grid-cols-1 lg:grid-cols-3 gap-6">
      <!-- Today's Upcoming Bookings (2 Cols) -->
      <div class="lg:col-span-2 glass-card rounded-2xl p-6 border border-slate-800/80">
        <div class="flex items-center justify-between mb-5">
          <div>
            <h3 class="text-base font-bold text-white">Today's Appointment Schedule</h3>
            <p class="text-xs text-slate-400 mt-0.5">Real-time schedule for {{ currentTenant.name }}</p>
          </div>
          <button 
            @click="$emit('switchTab', 'calendar')"
            class="text-xs text-indigo-400 hover:text-indigo-300 font-medium flex items-center gap-1"
          >
            <span>Open Calendar</span>
            <Icon name="chevron-right" size="13" />
          </button>
        </div>

        <div v-if="currentBookings.length === 0" class="text-center py-10 text-slate-500 text-xs">
          No appointments recorded yet.
        </div>

        <div v-else class="space-y-3">
          <div
            v-for="booking in currentBookings.slice(0, 5)"
            :key="booking.id"
            @click="$emit('selectBooking', booking)"
            class="cursor-pointer p-4 rounded-xl bg-slate-900/60 hover:bg-slate-800/70 border border-slate-800/80 hover:border-slate-700 flex flex-col sm:flex-row sm:items-center justify-between gap-3 transition-all"
          >
            <div class="flex items-start sm:items-center gap-3">
              <div 
                class="w-10 h-10 rounded-xl flex items-center justify-center text-xs font-bold text-white flex-shrink-0"
                :style="{ backgroundColor: currentTenant.accentColor }"
              >
                {{ booking.time.split(':')[0] }}h
              </div>
              <div>
                <div class="flex items-center gap-2">
                  <span class="text-xs font-bold text-white">{{ booking.clientName }}</span>
                  <span 
                    class="px-2 py-0.5 rounded text-[10px] font-semibold uppercase tracking-wider"
                    :class="[
                      booking.status === 'confirmed' ? 'bg-emerald-500/20 text-emerald-300' :
                      booking.status === 'pending' ? 'bg-amber-500/20 text-amber-300' :
                      booking.status === 'completed' ? 'bg-indigo-500/20 text-indigo-300' :
                      'bg-rose-500/20 text-rose-300'
                    ]"
                  >
                    {{ booking.status }}
                  </span>
                </div>
                <div class="text-xs text-slate-400 mt-0.5">
                  {{ booking.serviceName }} • <span class="text-slate-300">{{ booking.staffName }}</span>
                </div>
              </div>
            </div>

            <div class="flex items-center justify-between sm:justify-end gap-3 text-xs">
              <span class="font-mono text-slate-300 bg-slate-800 px-2.5 py-1 rounded-md">{{ booking.time }}</span>
              <span class="font-semibold text-white">${{ booking.price }}</span>
              <Icon name="chevron-right" size="14" class="text-slate-500" />
            </div>
          </div>
        </div>
      </div>

      <!-- Right Column: Tenant Direct Booking Link & Sync Status -->
      <div class="space-y-6">
        <!-- Live Tenant Portal Widget -->
        <div class="glass-card rounded-2xl p-6 border border-slate-800/80">
          <div class="flex items-center gap-2 text-xs font-bold text-white uppercase tracking-wider mb-2">
            <Icon name="globe" size="14" class="text-indigo-400" />
            <span>Client Booking URL</span>
          </div>
          <p class="text-xs text-slate-400 leading-relaxed">
            Your clients can book directly without creating an account through your branded portal:
          </p>

          <div class="mt-3 p-3 rounded-xl bg-slate-900 border border-slate-800 flex items-center justify-between text-xs">
            <span class="font-mono text-indigo-300 truncate">https://{{ currentTenant.subdomain }}/book</span>
            <button
              @click="copyUrl"
              class="ml-2 text-slate-400 hover:text-white p-1 rounded hover:bg-slate-800 transition-colors"
              title="Copy link"
            >
              <Icon :name="copied ? 'check' : 'copy'" size="14" :class="copied ? 'text-emerald-400' : ''" />
            </button>
          </div>

          <button
            @click="openPortal"
            class="mt-4 w-full py-2.5 rounded-xl text-xs font-semibold text-white bg-slate-800 hover:bg-slate-700 border border-slate-700 flex items-center justify-center gap-2 transition-colors"
          >
            <Icon name="external-link" size="14" />
            <span>Test Client Booking Experience</span>
          </button>
        </div>

        <!-- Multi-Tenant Sync & System Status -->
        <div class="glass-card rounded-2xl p-6 border border-slate-800/80">
          <h4 class="text-xs font-bold text-white uppercase tracking-wider mb-3">Sync & Integrations</h4>
          <div class="space-y-3 text-xs">
            <div class="flex items-center justify-between p-2.5 rounded-xl bg-slate-900/50 border border-slate-800">
              <div class="flex items-center gap-2 text-slate-300">
                <span class="w-2 h-2 rounded-full bg-emerald-400"></span>
                <span>Google Calendar Sync</span>
              </div>
              <span class="text-[11px] font-mono text-emerald-400">Connected</span>
            </div>

            <div class="flex items-center justify-between p-2.5 rounded-xl bg-slate-900/50 border border-slate-800">
              <div class="flex items-center gap-2 text-slate-300">
                <span class="w-2 h-2 rounded-full bg-emerald-400"></span>
                <span>Stripe Payment Gate</span>
              </div>
              <span class="text-[11px] font-mono text-emerald-400">Live</span>
            </div>

            <div class="flex items-center justify-between p-2.5 rounded-xl bg-slate-900/50 border border-slate-800">
              <div class="flex items-center gap-2 text-slate-300">
                <span class="w-2 h-2 rounded-full bg-emerald-400"></span>
                <span>SMS Reminders (Twilio)</span>
              </div>
              <span class="text-[11px] font-mono text-emerald-400">Active</span>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import Icon from '../common/Icon.vue'
import { useSlotly } from '../../composables/useSlotly'

defineEmits(['switchTab', 'selectBooking'])

const {
  currentTenant,
  currentBookings,
  computedStats,
  setView,
  showToast
} = useSlotly()

const copied = ref(false)

function copyUrl() {
  navigator.clipboard?.writeText(`https://${currentTenant.value.subdomain}/book`)
  copied.value = true
  showToast('Copied to Clipboard', `Portal link for ${currentTenant.value.name} copied!`)
  setTimeout(() => {
    copied.value = false
  }, 2000)
}

function openPortal() {
  setView('client-portal')
}
</script>
