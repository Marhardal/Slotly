<template>
  <div class="space-y-6">
    <div class="glass-card rounded-2xl p-4 border border-slate-800 flex flex-col sm:flex-row sm:items-center justify-between gap-4">
      <div>
        <h3 class="text-base font-bold text-white">Service Catalog & Offerings</h3>
        <p class="text-xs text-slate-400">Configure bookable treatments, sessions, duration, and pricing</p>
      </div>

      <button
        @click="showAddModal = true"
        class="px-4 py-2 rounded-xl text-xs font-semibold text-white bg-indigo-600 hover:bg-indigo-500 shadow-md shadow-indigo-600/30 flex items-center gap-1.5 transition-all self-start sm:self-auto"
      >
        <Icon name="plus" size="14" />
        <span>Add New Service</span>
      </button>
    </div>

    <!-- Services Grid -->
    <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
      <div
        v-for="service in currentServices"
        :key="service.id"
        class="glass-card rounded-2xl p-5 border border-slate-800 hover:border-slate-700 transition-all flex flex-col justify-between"
      >
        <div>
          <div class="flex items-start justify-between gap-3">
            <div>
              <span class="text-[10px] font-bold uppercase tracking-wider text-indigo-400 bg-indigo-950/60 px-2 py-0.5 rounded border border-indigo-500/20">
                {{ service.category }}
              </span>
              <h4 class="text-base font-bold text-white mt-1.5">{{ service.name }}</h4>
            </div>
            <div class="text-right">
              <span class="text-lg font-bold text-white">${{ service.price }}</span>
              <div class="text-[11px] text-slate-400">{{ service.durationMinutes }} mins</div>
            </div>
          </div>

          <p class="text-xs text-slate-300 mt-3 leading-relaxed">
            {{ service.description }}
          </p>
        </div>

        <div class="pt-4 mt-4 border-t border-slate-800 flex items-center justify-between">
          <div class="flex items-center gap-2">
            <span v-if="service.popular" class="text-[10px] font-semibold text-amber-300 bg-amber-500/10 px-2 py-0.5 rounded border border-amber-500/20">
              Featured / Popular
            </span>
            <span class="text-[11px] text-slate-500 font-mono">{{ service.id }}</span>
          </div>

          <button
            @click="deleteService(service.id)"
            class="text-xs text-rose-400 hover:text-rose-300 hover:bg-rose-500/10 px-2.5 py-1 rounded-lg transition-colors flex items-center gap-1"
          >
            <Icon name="x" size="13" />
            <span>Remove</span>
          </button>
        </div>
      </div>
    </div>

    <!-- Add Service Modal -->
    <div v-if="showAddModal" class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/70 backdrop-blur-sm">
      <div class="glass-card rounded-2xl max-w-lg w-full p-6 border border-slate-700 shadow-2xl">
        <div class="flex items-center justify-between pb-4 border-b border-slate-800">
          <h3 class="text-base font-bold text-white">Add New Bookable Service</h3>
          <button @click="showAddModal = false" class="text-slate-400 hover:text-white">
            <Icon name="x" size="18" />
          </button>
        </div>

        <form @submit.prevent="submitNewService" class="space-y-4 mt-4">
          <div>
            <label class="block text-xs font-medium text-slate-300 mb-1">Service Title *</label>
            <input
              v-model="newService.name"
              required
              type="text"
              placeholder="e.g. Express Lymphatic Drainage"
              class="w-full bg-slate-900 border border-slate-700 rounded-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
            />
          </div>

          <div class="grid grid-cols-3 gap-3">
            <div>
              <label class="block text-xs font-medium text-slate-300 mb-1">Category *</label>
              <input
                v-model="newService.category"
                required
                type="text"
                placeholder="e.g. Wellness"
                class="w-full bg-slate-900 border border-slate-700 rounded-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
              />
            </div>
            <div>
              <label class="block text-xs font-medium text-slate-300 mb-1">Duration (Min) *</label>
              <input
                v-model="newService.durationMinutes"
                required
                type="number"
                min="15"
                step="15"
                placeholder="60"
                class="w-full bg-slate-900 border border-slate-700 rounded-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
              />
            </div>
            <div>
              <label class="block text-xs font-medium text-slate-300 mb-1">Price ($ USD) *</label>
              <input
                v-model="newService.price"
                required
                type="number"
                min="0"
                placeholder="120"
                class="w-full bg-slate-900 border border-slate-700 rounded-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
              />
            </div>
          </div>

          <div>
            <label class="block text-xs font-medium text-slate-300 mb-1">Service Description</label>
            <textarea
              v-model="newService.description"
              rows="3"
              placeholder="Detail what client can expect, techniques used, benefits..."
              class="w-full bg-slate-900 border border-slate-700 rounded-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
            ></textarea>
          </div>

          <div class="flex items-center gap-2">
            <input
              id="popular"
              v-model="newService.popular"
              type="checkbox"
              class="rounded bg-slate-900 border-slate-700 text-indigo-600 focus:ring-0"
            />
            <label for="popular" class="text-xs text-slate-300">Highlight as Featured / Popular Service</label>
          </div>

          <div class="flex justify-end gap-3 pt-3 border-t border-slate-800">
            <button
              type="button"
              @click="showAddModal = false"
              class="px-4 py-2 text-xs font-medium text-slate-400 hover:text-white"
            >
              Cancel
            </button>
            <button
              type="submit"
              class="px-5 py-2 text-xs font-semibold text-white bg-indigo-600 hover:bg-indigo-500 rounded-xl shadow-lg shadow-indigo-600/30"
            >
              Save Service
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

const { currentServices, addService, deleteService } = useSlotly()

const showAddModal = ref(false)
const newService = ref({
  name: '',
  category: 'Wellness',
  durationMinutes: 60,
  price: 120,
  description: '',
  popular: false
})

function submitNewService() {
  addService(newService.value)
  showAddModal.value = false
  newService.value = {
    name: '',
    category: 'Wellness',
    durationMinutes: 60,
    price: 120,
    description: '',
    popular: false
  }
}
</script>
