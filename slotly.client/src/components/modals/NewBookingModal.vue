<template>
  <div v-if="isOpen" class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/70 backdrop-blur-sm">
    <div class="glass-card rounded-2xl max-w-lg w-full p-6 border border-slate-700/80 shadow-2xl relative">
      <div class="flex items-center justify-between pb-4 border-b border-slate-800">
        <div>
          <h3 class="text-base font-bold text-white">Create New Appointment</h3>
          <p class="text-xs text-slate-400">Create a booking request for this tenant</p>
        </div>
        <button @click="$emit('close')" class="text-slate-400 hover:text-white p-1 rounded-lg hover:bg-slate-800">
          <Icon name="x" size="18" />
        </button>
      </div>

      <form @submit.prevent="submitManualBooking" class="space-y-4 mt-4">
        <div>
          <label class="block text-xs font-medium text-slate-300 mb-1">Client Name *</label>
          <input
            v-model="name"
            required
            type="text"
            placeholder="e.g. Jessica Alba"
            class="w-full bg-slate-900 border border-slate-700 rounded-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
          />
        </div>

        <div class="grid grid-cols-2 gap-3">
          <div>
            <label class="block text-xs font-medium text-slate-300 mb-1">Email *</label>
            <input
              v-model="email"
              required
              type="email"
              placeholder="client@email.com"
              class="w-full bg-slate-900 border border-slate-700 rounded-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
            />
          </div>
          <div>
            <label class="block text-xs font-medium text-slate-300 mb-1">Phone</label>
            <input
              v-model="phone"
              type="tel"
              placeholder="+1 (555) 000-0000"
              class="w-full bg-slate-900 border border-slate-700 rounded-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
            />
          </div>
        </div>

        <div class="grid grid-cols-2 gap-3">
          <div>
            <label class="block text-xs font-medium text-slate-300 mb-1">Service *</label>
            <select
              v-model="serviceId"
              required
              class="w-full bg-slate-900 border border-slate-700 rounded-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
            >
              <option v-for="s in currentServices" :key="s.id" :value="s.id">
                {{ s.name }} (${{ s.price }})
              </option>
            </select>
          </div>
          <div>
            <label class="block text-xs font-medium text-slate-300 mb-1">Specialist *</label>
            <select
              v-model="staffId"
              required
              class="w-full bg-slate-900 border border-slate-700 rounded-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
            >
              <option v-for="st in currentStaff" :key="st.id" :value="st.id">
                {{ st.name }}
              </option>
            </select>
          </div>
        </div>

        <div class="grid grid-cols-2 gap-3">
          <div>
            <label class="block text-xs font-medium text-slate-300 mb-1">Date *</label>
            <input
              v-model="date"
              type="date"
              required
              class="w-full bg-slate-900 border border-slate-700 rounded-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
            />
          </div>
          <div>
            <label class="block text-xs font-medium text-slate-300 mb-1">Time Slot *</label>
            <select
              v-model="time"
              required
              class="w-full bg-slate-900 border border-slate-700 rounded-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
            >
              <option value="09:00 AM">09:00 AM</option>
              <option value="10:00 AM">10:00 AM</option>
              <option value="11:15 AM">11:15 AM</option>
              <option value="01:00 PM">01:00 PM</option>
              <option value="02:15 PM">02:15 PM</option>
              <option value="03:30 PM">03:30 PM</option>
              <option value="04:45 PM">04:45 PM</option>
              <option value="05:30 PM">05:30 PM</option>
            </select>
          </div>
        </div>

        <div>
          <label class="block text-xs font-medium text-slate-300 mb-1">Staff Notes</label>
          <input
            v-model="notes"
            type="text"
            placeholder="Walk-in client / phone booking details"
            class="w-full bg-slate-900 border border-slate-700 rounded-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
          />
        </div>

        <div class="flex items-center justify-end gap-3 pt-3 border-t border-slate-800">
          <button
            type="button"
            @click="$emit('close')"
            class="px-4 py-2 text-xs font-medium text-slate-400 hover:text-white"
          >
            Cancel
          </button>
          <button
            type="submit"
            :disabled="isSubmitting || currentServices.length === 0"
            class="px-5 py-2 text-xs font-semibold text-white bg-indigo-600 hover:bg-indigo-500 rounded-xl shadow-lg shadow-indigo-600/30"
          >
            {{ isSubmitting ? 'Saving...' : 'Create Appointment' }}
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { ref, watch } from 'vue'
import Icon from '../common/Icon.vue'
import { useSlotly } from '../../composables/useSlotly'

const props = defineProps({
  isOpen: Boolean
})

const emit = defineEmits(['close'])

const { currentServices, currentStaff, createBooking, showToast } = useSlotly()

const name = ref('')
const email = ref('')
const phone = ref('')
const serviceId = ref(currentServices.value[0]?.id || '')
const staffId = ref(currentStaff.value[0]?.id || '')
const today = new Date()
const date = ref(`${today.getFullYear()}-${String(today.getMonth() + 1).padStart(2, '0')}-${String(today.getDate()).padStart(2, '0')}`)
const time = ref('10:00 AM')
const notes = ref('')
const isSubmitting = ref(false)

watch(() => props.isOpen, (open) => {
  if (open) {
    if (currentServices.value.length) serviceId.value = currentServices.value[0].id
    if (currentStaff.value.length) staffId.value = currentStaff.value[0].id
  }
})

async function submitManualBooking() {
  const service = currentServices.value.find(s => s.id === serviceId.value)
  const staff = currentStaff.value.find(st => st.id === staffId.value)
  if (!service) {
    showToast('Could Not Create Booking', 'Select a service that is available in the backend catalog.', 'error')
    return
  }

  isSubmitting.value = true
  try {
    await createBooking({
      clientName: name.value,
      clientEmail: email.value,
      clientPhone: phone.value,
      serviceId: serviceId.value,
      serviceName: service.name,
      staffId: staffId.value,
      staffName: staff?.name || 'Staff Member',
      date: date.value,
      time: time.value,
      duration: service.durationMinutes,
      price: service.price,
      notes: notes.value
    })

    name.value = ''
    email.value = ''
    phone.value = ''
    notes.value = ''
    emit('close')
  } catch (error) {
    showToast('Could Not Create Booking', error.message, 'error')
  } finally {
    isSubmitting.value = false
  }
}
</script>
