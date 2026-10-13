<template>
  <div class="space-y-4">
    <!-- Calendar Toolbar -->
    <div class="glass-card rounded-2xl p-4 border border-slate-800 flex flex-col sm:flex-row sm:items-center justify-between gap-4">
      <div class="flex items-center gap-3">
        <div class="flex items-center bg-slate-900 border border-slate-700/80 rounded-xl p-1 text-xs">
          <button
            @click="calendarViewMode = 'week'"
            class="px-3 py-1 rounded-lg font-medium transition-all"
            :class="calendarViewMode === 'week' ? 'bg-indigo-600 text-white' : 'text-slate-400 hover:text-white'"
          >
            Week View
          </button>
          <button
            @click="calendarViewMode = 'day'"
            class="px-3 py-1 rounded-lg font-medium transition-all"
            :class="calendarViewMode === 'day' ? 'bg-indigo-600 text-white' : 'text-slate-400 hover:text-white'"
          >
            Day View
          </button>
        </div>

        <span class="text-sm font-bold text-white">October 2026</span>
      </div>

      <!-- Filters & Actions -->
      <div class="flex items-center gap-3">
        <!-- Staff Filter -->
        <select
          v-model="selectedStaffFilter"
          class="bg-slate-900 border border-slate-700/80 rounded-xl px-3 py-1.5 text-xs text-slate-200 focus:outline-none focus:border-indigo-500"
        >
          <option value="all">All Specialists ({{ currentStaff.length }})</option>
          <option v-for="staff in currentStaff" :key="staff.id" :value="staff.name">
            {{ staff.name }}
          </option>
        </select>

        <button
          @click="$emit('openNewBooking')"
          class="px-3.5 py-1.5 rounded-xl text-xs font-semibold text-white bg-indigo-600 hover:bg-indigo-500 shadow-md shadow-indigo-600/30 flex items-center gap-1.5 transition-all"
        >
          <Icon name="plus" size="14" />
          <span>New Appointment</span>
        </button>
      </div>
    </div>

    <!-- WEEK VIEW SCHEDULE GRID -->
    <div v-if="calendarViewMode === 'week'" class="glass-card rounded-2xl border border-slate-800 overflow-x-auto shadow-2xl">
      <div class="min-w-[760px]">
        <!-- Days Header Row -->
        <div class="grid grid-cols-8 border-b border-slate-800 bg-slate-900/80 text-xs">
          <div class="p-3 text-slate-500 font-medium text-center border-r border-slate-800">
            Time
          </div>
          <div
            v-for="day in weekDays"
            :key="day.dateString"
            class="p-3 text-center border-r last:border-r-0 border-slate-800"
            :class="day.isToday ? 'bg-indigo-950/30' : ''"
          >
            <div class="text-[11px] font-semibold uppercase tracking-wider" :class="day.isToday ? 'text-indigo-400' : 'text-slate-400'">
              {{ day.dayName }}
            </div>
            <div class="text-sm font-bold text-white mt-0.5">
              {{ day.dayNum }} Oct
            </div>
          </div>
        </div>

        <!-- Schedule Hours Grid -->
        <div class="divide-y divide-slate-800/60 text-xs">
          <div
            v-for="hour in hoursList"
            :key="hour"
            class="grid grid-cols-8 min-h-[72px]"
          >
            <!-- Hour Label -->
            <div class="p-2.5 text-slate-500 font-mono text-[11px] text-center border-r border-slate-800 bg-slate-950/40 flex items-start justify-center">
              {{ hour }}
            </div>

            <!-- Day Columns -->
            <div
              v-for="day in weekDays"
              :key="day.dateString + hour"
              class="border-r last:border-r-0 border-slate-800/80 p-1 relative hover:bg-slate-800/20 transition-colors group"
            >
              <!-- Render matching appointments in this slot -->
              <div
                v-for="b in getBookingsForSlot(day.dateString, hour)"
                :key="b.id"
                @click.stop="$emit('selectBooking', b)"
                class="cursor-pointer p-2 rounded-lg text-[11px] mb-1 shadow-md border transition-all transform hover:scale-[1.02]"
                :class="[
                  b.status === 'confirmed' ? 'bg-emerald-950/80 border-emerald-500/50 text-emerald-200' :
                  b.status === 'pending' ? 'bg-amber-950/80 border-amber-500/50 text-amber-200' :
                  b.status === 'completed' ? 'bg-indigo-950/80 border-indigo-500/50 text-indigo-200' :
                  'bg-rose-950/80 border-rose-500/50 text-rose-200'
                ]"
              >
                <div class="flex items-center justify-between font-bold truncate">
                  <span>{{ b.clientName }}</span>
                  <span class="text-[10px] opacity-80">${{ b.price }}</span>
                </div>
                <div class="truncate text-[10px] opacity-90 mt-0.5">
                  {{ b.serviceName }}
                </div>
                <div class="text-[9px] opacity-75 mt-0.5 font-mono">
                  {{ b.staffName }}
                </div>
              </div>

              <!-- Quick empty slot click to book -->
              <button
                v-if="getBookingsForSlot(day.dateString, hour).length === 0"
                @click="$emit('openNewBooking')"
                class="w-full h-full opacity-0 group-hover:opacity-100 flex items-center justify-center text-[10px] text-slate-500 hover:text-indigo-400 transition-opacity"
              >
                <span class="bg-slate-800/80 px-2 py-1 rounded border border-slate-700">+ Book</span>
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>

    <!-- DAY VIEW -->
    <div v-else class="glass-card rounded-2xl border border-slate-800 p-6 shadow-2xl">
      <div class="flex items-center justify-between mb-4 pb-3 border-b border-slate-800">
        <div>
          <h3 class="text-base font-bold text-white">Wednesday, October 14, 2026</h3>
          <p class="text-xs text-slate-400">Detailed timeline of scheduled appointments today</p>
        </div>
        <span class="text-xs font-mono px-3 py-1 rounded bg-slate-800 text-indigo-300">
          {{ getBookingsForDay('2026-10-14').length }} Appointments
        </span>
      </div>

      <div class="space-y-3">
        <div
          v-for="booking in getBookingsForDay('2026-10-14')"
          :key="booking.id"
          @click="$emit('selectBooking', booking)"
          class="cursor-pointer p-4 rounded-xl border flex items-center justify-between gap-4 transition-all hover:border-indigo-500"
          :class="[
            booking.status === 'confirmed' ? 'bg-emerald-950/30 border-emerald-500/30' :
            booking.status === 'pending' ? 'bg-amber-950/30 border-amber-500/30' :
            'bg-slate-900/60 border-slate-800'
          ]"
        >
          <div class="flex items-center gap-4">
            <div class="text-center font-mono text-xs font-bold text-indigo-300 bg-slate-900 px-3 py-2 rounded-lg border border-slate-800">
              {{ booking.time }}
            </div>
            <div>
              <div class="flex items-center gap-2">
                <span class="text-sm font-bold text-white">{{ booking.clientName }}</span>
                <span class="text-xs text-slate-400 font-mono">({{ booking.clientPhone }})</span>
              </div>
              <div class="text-xs text-slate-300 mt-0.5">
                {{ booking.serviceName }} • Assigned to <span class="text-white font-medium">{{ booking.staffName }}</span>
              </div>
            </div>
          </div>

          <div class="flex items-center gap-3">
            <span class="text-sm font-bold text-white">${{ booking.price }}</span>
            <span 
              class="px-2.5 py-1 rounded-full text-xs font-semibold capitalize"
              :class="booking.status === 'confirmed' ? 'bg-emerald-500/20 text-emerald-300' : 'bg-amber-500/20 text-amber-300'"
            >
              {{ booking.status }}
            </span>
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

defineEmits(['selectBooking', 'openNewBooking'])

const { currentBookings, currentStaff } = useSlotly()

const calendarViewMode = ref('week')
const selectedStaffFilter = ref('all')

const weekDays = [
  { dateString: '2026-10-12', dayName: 'Mon', dayNum: '12', isToday: false },
  { dateString: '2026-10-13', dayName: 'Tue', dayNum: '13', isToday: false },
  { dateString: '2026-10-14', dayName: 'Wed', dayNum: '14', isToday: true },
  { dateString: '2026-10-15', dayName: 'Thu', dayNum: '15', isToday: false },
  { dateString: '2026-10-16', dayName: 'Fri', dayNum: '16', isToday: false },
  { dateString: '2026-10-17', dayName: 'Sat', dayNum: '17', isToday: false },
  { dateString: '2026-10-18', dayName: 'Sun', dayNum: '18', isToday: false }
]

const hoursList = [
  '09:00 AM',
  '10:00 AM',
  '11:00 AM',
  '12:00 PM',
  '01:00 PM',
  '02:00 PM',
  '03:00 PM',
  '04:00 PM',
  '05:00 PM'
]

function getBookingsForSlot(dateStr, hourStr) {
  const hourPrefix = hourStr.split(':')[0]
  const hourAmPm = hourStr.split(' ')[1]

  return currentBookings.value.filter(b => {
    if (selectedStaffFilter.value !== 'all' && b.staffName !== selectedStaffFilter.value) {
      return false
    }
    if (b.date !== dateStr) return false
    
    // Approximate match on hour and AM/PM
    const bHourPrefix = b.time.split(':')[0]
    const bAmPm = b.time.split(' ')[1]
    return bHourPrefix === hourPrefix && bAmPm === hourAmPm
  })
}

function getBookingsForDay(dateStr) {
  return currentBookings.value.filter(b => b.date === dateStr)
}
</script>
