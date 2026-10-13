<template>
  <div class="space-y-6">
    <div class="glass-card rounded-2xl p-4 border border-slate-800 flex flex-col sm:flex-row sm:items-center justify-between gap-4">
      <div>
        <h3 class="text-base font-bold text-white">Practitioners & Specialists</h3>
        <p class="text-xs text-slate-400">Manage provider profiles, schedules, and service assignments</p>
      </div>

      <button
        @click="showAddStaffModal = true"
        class="px-4 py-2 rounded-xl text-xs font-semibold text-white bg-indigo-600 hover:bg-indigo-500 shadow-md shadow-indigo-600/30 flex items-center gap-1.5 transition-all self-start sm:self-auto"
      >
        <Icon name="plus" size="14" />
        <span>Add Specialist</span>
      </button>
    </div>

    <!-- Staff Cards Grid -->
    <div class="grid grid-cols-1 md:grid-cols-3 gap-4">
      <div
        v-for="staff in currentStaff"
        :key="staff.id"
        class="glass-card rounded-2xl p-5 border border-slate-800 hover:border-slate-700 transition-all flex flex-col justify-between"
      >
        <div>
          <div class="flex items-center gap-3">
            <img :src="staff.avatar" :alt="staff.name" class="w-12 h-12 rounded-xl object-cover border border-slate-700 shadow-md" />
            <div>
              <h4 class="text-sm font-bold text-white">{{ staff.name }}</h4>
              <p class="text-[11px] text-slate-400 mt-0.5">{{ staff.role }}</p>
              <div class="flex items-center gap-1 text-[11px] text-amber-400 mt-1">
                <Icon name="star" size="11" class="fill-amber-400" />
                <span>{{ staff.rating }} rating</span>
              </div>
            </div>
          </div>

          <div class="mt-4 pt-3 border-t border-slate-800/80">
            <span class="text-[10px] uppercase font-bold text-slate-400 tracking-wider">Available Days</span>
            <div class="flex flex-wrap gap-1.5 mt-2">
              <span
                v-for="day in ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat']"
                :key="day"
                class="px-2 py-0.5 rounded text-[10px] font-medium"
                :class="staff.availableDays?.includes(day) ? 'bg-indigo-500/20 text-indigo-300 border border-indigo-500/30 font-semibold' : 'bg-slate-900/60 text-slate-600'"
              >
                {{ day }}
              </span>
            </div>
          </div>
        </div>

        <div class="pt-4 mt-4 border-t border-slate-800/80 flex items-center justify-between text-xs text-slate-400">
          <span class="flex items-center gap-1.5">
            <span class="w-2 h-2 rounded-full bg-emerald-400"></span>
            <span>Accepting Bookings</span>
          </span>
          <span class="font-mono text-[10px]">{{ staff.id }}</span>
        </div>
      </div>
    </div>

    <!-- Add Staff Modal -->
    <div v-if="showAddStaffModal" class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/70 backdrop-blur-sm">
      <div class="glass-card rounded-2xl max-w-md w-full p-6 border border-slate-700 shadow-2xl">
        <div class="flex items-center justify-between pb-4 border-b border-slate-800">
          <h3 class="text-base font-bold text-white">Add New Practitioner</h3>
          <button @click="showAddStaffModal = false" class="text-slate-400 hover:text-white">
            <Icon name="x" size="18" />
          </button>
        </div>

        <form @submit.prevent="submitStaff" class="space-y-4 mt-4">
          <div>
            <label class="block text-xs font-medium text-slate-300 mb-1">Full Name *</label>
            <input
              v-model="newStaff.name"
              required
              type="text"
              placeholder="e.g. Dr. Julian Vance"
              class="w-full bg-slate-900 border border-slate-700 rounded-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
            />
          </div>

          <div>
            <label class="block text-xs font-medium text-slate-300 mb-1">Professional Title / Specialty *</label>
            <input
              v-model="newStaff.role"
              required
              type="text"
              placeholder="e.g. Senior Neuromuscular Specialist"
              class="w-full bg-slate-900 border border-slate-700 rounded-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
            />
          </div>

          <div>
            <label class="block text-xs font-medium text-slate-300 mb-1">Photo Avatar URL (Optional)</label>
            <input
              v-model="newStaff.avatar"
              type="url"
              placeholder="https://images.unsplash.com/..."
              class="w-full bg-slate-900 border border-slate-700 rounded-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
            />
          </div>

          <div class="flex justify-end gap-3 pt-3 border-t border-slate-800">
            <button
              type="button"
              @click="showAddStaffModal = false"
              class="px-4 py-2 text-xs font-medium text-slate-400 hover:text-white"
            >
              Cancel
            </button>
            <button
              type="submit"
              class="px-5 py-2 text-xs font-semibold text-white bg-indigo-600 hover:bg-indigo-500 rounded-xl shadow-lg shadow-indigo-600/30"
            >
              Add Specialist
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import Icon from '../common/Icon.vue'
import { useSlotly } from '../../composables/useSlotly'

const { currentStaff, addStaffMember } = useSlotly()

const showAddStaffModal = ref(false)
const newStaff = ref({
  name: '',
  role: '',
  avatar: ''
})

function submitStaff() {
  addStaffMember({
    name: newStaff.value.name,
    role: newStaff.value.role,
    avatar: newStaff.value.avatar || 'https://images.unsplash.com/photo-1534528741775-53994a69daeb?w=150&auto=format&fit=crop&q=80',
    availableDays: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri']
  })
  showAddStaffModal.value = false
  newStaff.value = { name: '', role: '', avatar: '' }
}
</script>
