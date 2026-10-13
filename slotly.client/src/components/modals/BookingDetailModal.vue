<template>
  <div v-if="booking" class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/70 backdrop-blur-sm">
    <div class="glass-card rounded-2xl max-w-lg w-full p-6 border border-slate-700/80 shadow-2xl relative max-h-[90vh] overflow-y-auto">
      <div class="flex items-center justify-between pb-4 border-b border-slate-800">
        <div class="flex items-center gap-2.5">
          <span 
            class="w-3 h-3 rounded-full"
            :class="[
              booking.status === 'confirmed' ? 'bg-emerald-400' :
              booking.status === 'pending' ? 'bg-amber-400' :
              booking.status === 'completed' ? 'bg-indigo-400' :
              'bg-rose-400'
            ]"
          ></span>
          <div>
            <h3 class="text-base font-bold text-white">Booking Details #{{ booking.id }}</h3>
            <p class="text-xs text-slate-400">Created on {{ formatDateTime(booking.createdAt) }}</p>
          </div>
        </div>
        <button @click="$emit('close')" class="text-slate-400 hover:text-white p-1 rounded-lg hover:bg-slate-800">
          <Icon name="x" size="18" />
        </button>
      </div>

      <div class="space-y-4 my-5 text-xs">
        <!-- Client Profile Box -->
        <div class="bg-slate-900/80 p-4 rounded-xl border border-slate-800">
          <div class="text-[11px] uppercase tracking-wider text-slate-400 font-semibold mb-2">Client Information</div>
          <div class="flex items-center gap-3">
            <div class="w-10 h-10 rounded-full bg-indigo-600/20 text-indigo-300 font-bold flex items-center justify-center text-sm border border-indigo-500/30">
              {{ booking.clientName.charAt(0) }}
            </div>
            <div>
              <div class="text-sm font-bold text-white">{{ booking.clientName }}</div>
              <div class="text-slate-400 flex items-center gap-2 mt-0.5">
                <span>{{ booking.clientEmail }}</span>
                <span>•</span>
                <span>{{ booking.clientPhone }}</span>
              </div>
            </div>
          </div>
        </div>

        <!-- Appointment Info Grid -->
        <div class="grid grid-cols-2 gap-3">
          <div class="p-3 bg-slate-900/60 rounded-xl border border-slate-800">
            <span class="text-slate-400">Service</span>
            <div class="font-bold text-white mt-1">{{ booking.serviceName }}</div>
            <div class="text-indigo-400 font-semibold mt-0.5">${{ booking.price }} USD</div>
          </div>

          <div class="p-3 bg-slate-900/60 rounded-xl border border-slate-800">
            <span class="text-slate-400">Practitioner</span>
            <div class="font-bold text-white mt-1">{{ booking.staffName }}</div>
            <div class="text-slate-400 mt-0.5">{{ booking.duration }} mins scheduled</div>
          </div>

          <div class="p-3 bg-slate-900/60 rounded-xl border border-slate-800">
            <span class="text-slate-400">Date</span>
            <div class="font-bold text-white mt-1">{{ booking.date }}</div>
          </div>

          <div class="p-3 bg-slate-900/60 rounded-xl border border-slate-800">
            <span class="text-slate-400">Time Slot</span>
            <div class="font-bold text-white mt-1">{{ booking.time }}</div>
          </div>
        </div>

        <!-- ITEMIZED TRANSACTION BILLING LEDGER -->
        <div class="bg-slate-900/80 p-4 rounded-xl border border-slate-800 space-y-2">
          <div class="flex items-center justify-between pb-2 border-b border-slate-800">
            <span class="text-[11px] uppercase tracking-wider text-slate-400 font-bold flex items-center gap-1.5">
              <Icon name="receipt" size="13" class="text-indigo-400" />
              <span>Transaction & Payment Breakdown</span>
            </span>
            <span class="font-mono text-indigo-300 text-[11px]">{{ booking.pricing?.transactionId || 'TXN-DIRECT' }}</span>
          </div>

          <div class="flex justify-between text-slate-300">
            <span>Base Service Rate</span>
            <span class="font-mono">${{ Number(booking.pricing?.basePrice || booking.price).toFixed(2) }}</span>
          </div>

          <div 
            v-for="addon in (booking.pricing?.addons || [])" 
            :key="addon.name"
            class="flex justify-between text-slate-400 text-[11px]"
          >
            <span>+ Addon: {{ addon.name }}</span>
            <span class="font-mono text-slate-300">+${{ Number(addon.price).toFixed(2) }}</span>
          </div>

          <div v-if="booking.pricing?.discount > 0" class="flex justify-between text-emerald-400 text-[11px]">
            <span>Promo Discount ({{ booking.pricing?.discountCode }})</span>
            <span class="font-mono">-${{ Number(booking.pricing?.discount).toFixed(2) }}</span>
          </div>

          <div v-if="booking.pricing?.tax > 0" class="flex justify-between text-slate-400 text-[11px]">
            <span>Sales Tax</span>
            <span class="font-mono">+${{ Number(booking.pricing?.tax).toFixed(2) }}</span>
          </div>

          <div v-if="booking.pricing?.processingFee > 0" class="flex justify-between text-slate-400 text-[11px]">
            <span>Processing Fee</span>
            <span class="font-mono">+${{ Number(booking.pricing?.processingFee).toFixed(2) }}</span>
          </div>

          <div v-if="booking.pricing?.tip > 0" class="flex justify-between text-slate-300 text-[11px]">
            <span>Tip / Gratuity</span>
            <span class="font-mono">+${{ Number(booking.pricing?.tip).toFixed(2) }}</span>
          </div>

          <div class="pt-2 border-t border-slate-800 flex justify-between font-bold text-white">
            <span>Total Transaction Amount</span>
            <span class="font-mono text-indigo-300 text-sm">${{ Number(booking.pricing?.grandTotal || booking.price).toFixed(2) }}</span>
          </div>

          <!-- Payment status summary -->
          <div class="pt-2 border-t border-slate-800 flex items-center justify-between text-[11px]">
            <div class="text-slate-400">
              Payment Method: <span class="text-white font-medium">{{ booking.pricing?.paymentMethod || 'Credit Card' }}</span>
            </div>
            <div>
              <span v-if="booking.pricing?.balanceDue > 0" class="text-amber-400 font-semibold">
                Deposit Paid: ${{ Number(booking.pricing?.depositAmount).toFixed(2) }} (Due: ${{ Number(booking.pricing?.balanceDue).toFixed(2) }})
              </span>
              <span v-else class="text-emerald-400 font-semibold">
                Paid in Full (${{ Number(booking.pricing?.grandTotal || booking.price).toFixed(2) }})
              </span>
            </div>
          </div>
        </div>

        <!-- Notes -->
        <div v-if="booking.notes" class="p-3 bg-slate-900/60 rounded-xl border border-slate-800">
          <span class="text-slate-400">Client / Internal Notes</span>
          <p class="text-slate-200 mt-1 leading-relaxed">{{ booking.notes }}</p>
        </div>

        <!-- Status Controller -->
        <div class="p-3 bg-slate-900/80 rounded-xl border border-slate-800 flex items-center justify-between">
          <div>
            <span class="text-slate-400">Current Status</span>
            <div class="font-bold text-white capitalize mt-0.5">{{ booking.status }}</div>
          </div>
          <div class="flex items-center gap-1.5">
            <button
              v-if="booking.status !== 'confirmed'"
              @click="setStatus('confirmed')"
              class="px-2.5 py-1 rounded-lg bg-emerald-500/20 text-emerald-300 hover:bg-emerald-500/30 font-medium"
            >
              Mark Confirmed
            </button>
            <button
              v-if="booking.status !== 'completed'"
              @click="setStatus('completed')"
              class="px-2.5 py-1 rounded-lg bg-indigo-500/20 text-indigo-300 hover:bg-indigo-500/30 font-medium"
            >
              Mark Completed
            </button>
            <button
              v-if="booking.status !== 'cancelled'"
              @click="setStatus('cancelled')"
              class="px-2.5 py-1 rounded-lg bg-rose-500/20 text-rose-300 hover:bg-rose-500/30 font-medium"
            >
              Cancel
            </button>
          </div>
        </div>
      </div>

      <div class="flex justify-end pt-3 border-t border-slate-800">
        <button
          @click="$emit('close')"
          class="px-5 py-2 text-xs font-semibold text-white bg-slate-800 hover:bg-slate-700 rounded-xl transition-colors"
        >
          Close
        </button>
      </div>
    </div>
  </div>
</template>

<script setup>
import Icon from '../common/Icon.vue'
import { useSlotly } from '../../composables/useSlotly'

const props = defineProps({
  booking: Object
})

const emit = defineEmits(['close'])

const { updateBookingStatus } = useSlotly()

function setStatus(status) {
  if (props.booking) {
    updateBookingStatus(props.booking.id, status)
  }
}

function formatDateTime(iso) {
  if (!iso) return 'Recent'
  try {
    return new Date(iso).toLocaleDateString('en-US', { month: 'short', day: 'numeric', hour: '2-digit', minute: '2-digit' })
  } catch {
    return iso
  }
}
</script>
