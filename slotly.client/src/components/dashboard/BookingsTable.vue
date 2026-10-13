<template>
  <div class="space-y-4">
    <!-- Controls Header -->
    <div class="glass-card rounded-2xl p-4 border border-slate-800 flex flex-col md:flex-row md:items-center justify-between gap-4">
      <!-- Search Input -->
      <div class="relative flex-1 max-w-md">
        <Icon name="search" size="16" class="absolute left-3.5 top-1/2 -translate-y-1/2 text-slate-400" />
        <input
          v-model="searchQuery"
          type="text"
          placeholder="Search by client name, email, or service..."
          class="w-full bg-slate-900 border border-slate-700/80 rounded-xl pl-10 pr-4 py-2 text-xs text-white placeholder-slate-500 focus:outline-none focus:border-indigo-500"
        />
      </div>

      <!-- Status Filters & Action -->
      <div class="flex items-center flex-wrap gap-2">
        <div class="flex items-center bg-slate-900 border border-slate-700/80 rounded-xl p-1 text-xs">
          <button
            v-for="status in ['all', 'confirmed', 'pending', 'completed', 'cancelled']"
            :key="status"
            @click="statusFilter = status"
            class="px-2.5 py-1 rounded-lg capitalize font-medium transition-all"
            :class="statusFilter === status ? 'bg-indigo-600 text-white' : 'text-slate-400 hover:text-white'"
          >
            {{ status }}
          </button>
        </div>

        <button
          @click="$emit('openNewBooking')"
          class="px-3.5 py-2 rounded-xl text-xs font-semibold text-white bg-indigo-600 hover:bg-indigo-500 shadow-md shadow-indigo-600/30 flex items-center gap-1.5 transition-all"
        >
          <Icon name="plus" size="14" />
          <span>New Booking</span>
        </button>
      </div>
    </div>

    <!-- Bookings Table -->
    <div class="glass-card rounded-2xl border border-slate-800 overflow-hidden shadow-2xl">
      <div class="overflow-x-auto">
        <table class="w-full text-left text-xs border-collapse">
          <thead>
            <tr class="border-b border-slate-800 bg-slate-900/90 text-slate-400 font-semibold uppercase tracking-wider text-[11px]">
              <th class="p-4">Client</th>
              <th class="p-4">Service & Specialist</th>
              <th class="p-4">Date & Time</th>
              <th class="p-4">Transaction / Billing</th>
              <th class="p-4">Status</th>
              <th class="p-4 text-right">Actions</th>
            </tr>
          </thead>
          <tbody class="divide-y divide-slate-800/60">
            <tr
              v-for="booking in filteredBookings"
              :key="booking.id"
              @click="$emit('selectBooking', booking)"
              class="hover:bg-slate-800/40 cursor-pointer transition-colors"
            >
              <!-- Client Name & Contact -->
              <td class="p-4">
                <div class="flex items-center gap-3">
                  <div class="w-8 h-8 rounded-full bg-slate-800 text-indigo-300 font-bold flex items-center justify-center text-xs border border-slate-700">
                    {{ booking.clientName.charAt(0) }}
                  </div>
                  <div>
                    <div class="font-bold text-white">{{ booking.clientName }}</div>
                    <div class="text-[11px] text-slate-400">{{ booking.clientEmail }}</div>
                  </div>
                </div>
              </td>

              <!-- Service & Staff -->
              <td class="p-4">
                <div class="font-medium text-slate-200">{{ booking.serviceName }}</div>
                <div class="text-[11px] text-slate-400 flex items-center gap-1 mt-0.5">
                  <Icon name="user" size="11" class="text-slate-500" />
                  <span>{{ booking.staffName }}</span>
                  <span v-if="booking.pricing?.addons?.length" class="text-indigo-400">
                    (+{{ booking.pricing.addons.length }} add-on{{ booking.pricing.addons.length > 1 ? 's' : '' }})
                  </span>
                </div>
              </td>

              <!-- Date & Time -->
              <td class="p-4">
                <div class="font-semibold text-white">{{ booking.date }}</div>
                <div class="text-[11px] font-mono text-indigo-300 mt-0.5">{{ booking.time }} ({{ booking.duration }}m)</div>
              </td>

              <!-- Transaction / Billing -->
              <td class="p-4">
                <div class="font-bold text-white font-mono">
                  ${{ Number(booking.pricing?.grandTotal || booking.price).toFixed(2) }}
                </div>
                <div class="text-[10px] mt-0.5">
                  <span v-if="booking.pricing?.balanceDue > 0" class="text-amber-400 font-medium">
                    Dep: ${{ Number(booking.pricing?.depositAmount).toFixed(2) }} (Due: ${{ Number(booking.pricing?.balanceDue).toFixed(2) }})
                  </span>
                  <span v-else-if="booking.pricing?.paymentOption === 'in_person'" class="text-slate-400">
                    Pay at Venue
                  </span>
                  <span v-else class="text-emerald-400 font-medium">
                    ✓ Paid in Full
                  </span>
                </div>
              </td>

              <!-- Status -->
              <td class="p-4">
                <span
                  class="inline-flex items-center gap-1.5 px-2.5 py-0.5 rounded-full text-[11px] font-semibold capitalize border"
                  :class="[
                    booking.status === 'confirmed' ? 'bg-emerald-500/10 border-emerald-500/30 text-emerald-300' :
                    booking.status === 'pending' ? 'bg-amber-500/10 border-amber-500/30 text-amber-300' :
                    booking.status === 'completed' ? 'bg-indigo-500/10 border-indigo-500/30 text-indigo-300' :
                    'bg-rose-500/10 border-rose-500/30 text-rose-300'
                  ]"
                >
                  <span 
                    class="w-1.5 h-1.5 rounded-full"
                    :class="[
                      booking.status === 'confirmed' ? 'bg-emerald-400' :
                      booking.status === 'pending' ? 'bg-amber-400' :
                      booking.status === 'completed' ? 'bg-indigo-400' :
                      'bg-rose-400'
                    ]"
                  ></span>
                  {{ booking.status }}
                </span>
              </td>

              <!-- Actions -->
              <td class="p-4 text-right" @click.stop>
                <div class="flex items-center justify-end gap-1.5">
                  <button
                    @click="$emit('selectBooking', booking)"
                    class="p-1.5 rounded-lg text-slate-400 hover:text-white hover:bg-slate-800 transition-colors"
                    title="View Receipt & Details"
                  >
                    <Icon name="receipt" size="15" />
                  </button>
                  <button
                    v-if="booking.status === 'pending'"
                    @click="updateBookingStatus(booking.id, 'confirmed')"
                    class="px-2 py-1 rounded-md text-[11px] font-semibold bg-emerald-500/20 text-emerald-300 hover:bg-emerald-500/30"
                  >
                    Confirm
                  </button>
                </div>
              </td>
            </tr>

            <tr v-if="filteredBookings.length === 0">
              <td colspan="6" class="p-8 text-center text-slate-500 text-xs">
                No bookings found matching current filters.
              </td>
            </tr>
          </tbody>
        </table>
      </div>

      <div class="p-4 border-t border-slate-800 bg-slate-900/60 flex items-center justify-between text-xs text-slate-400">
        <span>Showing {{ filteredBookings.length }} of {{ currentBookings.length }} bookings</span>
        <span class="font-mono text-[11px]">Tenant ID: {{ currentTenant.id }}</span>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import Icon from '../common/Icon.vue'
import { useSlotly } from '../../composables/useSlotly'

defineEmits(['selectBooking', 'openNewBooking'])

const { currentTenant, currentBookings, updateBookingStatus } = useSlotly()

const searchQuery = ref('')
const statusFilter = ref('all')

const filteredBookings = computed(() => {
  return currentBookings.value.filter(b => {
    // Status filter
    if (statusFilter.value !== 'all' && b.status !== statusFilter.value) {
      return false
    }

    // Search query filter
    if (searchQuery.value.trim() !== '') {
      const q = searchQuery.value.toLowerCase()
      const matchClient = b.clientName.toLowerCase().includes(q)
      const matchEmail = b.clientEmail.toLowerCase().includes(q)
      const matchService = b.serviceName.toLowerCase().includes(q)
      return matchClient || matchEmail || matchService
    }

    return true
  })
})
</script>
