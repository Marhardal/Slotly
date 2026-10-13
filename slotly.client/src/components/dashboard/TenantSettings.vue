<template>
  <div class="space-y-6">
    <div class="glass-card rounded-2xl p-4 border border-slate-800 flex items-center justify-between">
      <div>
        <h3 class="text-base font-bold text-white">Tenant Branding & Transaction Policies</h3>
        <p class="text-xs text-slate-400">Configure white-label portal, transaction tax rates, deposit rules, and brand colors</p>
      </div>

      <button
        @click="saveAllSettings"
        class="px-5 py-2 rounded-xl text-xs font-semibold text-white bg-indigo-600 hover:bg-indigo-500 shadow-md shadow-indigo-600/30 flex items-center gap-1.5 transition-all"
      >
        <Icon name="check" size="14" />
        <span>Save Changes</span>
      </button>
    </div>

    <div class="grid grid-cols-1 lg:grid-cols-2 gap-6">
      <!-- Brand & White-Label Customization -->
      <div class="glass-card rounded-2xl p-6 border border-slate-800 space-y-4">
        <h4 class="text-sm font-bold text-white flex items-center gap-2">
          <Icon name="sparkles" size="16" class="text-indigo-400" />
          <span>Brand Identity & Subdomain</span>
        </h4>

        <div>
          <label class="block text-xs font-medium text-slate-300 mb-1">Organization Name</label>
          <input
            v-model="form.name"
            type="text"
            class="w-full bg-slate-900 border border-slate-700 rounded-xl px-3.5 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
          />
        </div>

        <div>
          <label class="block text-xs font-medium text-slate-300 mb-1">Brand Tagline</label>
          <input
            v-model="form.tagline"
            type="text"
            class="w-full bg-slate-900 border border-slate-700 rounded-xl px-3.5 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
          />
        </div>

        <div class="grid grid-cols-2 gap-3">
          <div>
            <label class="block text-xs font-medium text-slate-300 mb-1">Subdomain Slug</label>
            <div class="flex items-center">
              <input
                v-model="form.slug"
                type="text"
                class="w-full bg-slate-900 border border-slate-700 rounded-l-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
              />
              <span class="bg-slate-800 border-y border-r border-slate-700 text-slate-400 text-xs px-2.5 py-2 rounded-r-xl">.slotly.io</span>
            </div>
          </div>

          <div>
            <label class="block text-xs font-medium text-slate-300 mb-1">Category Badge</label>
            <input
              v-model="form.badge"
              type="text"
              class="w-full bg-slate-900 border border-slate-700 rounded-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
            />
          </div>
        </div>

        <!-- Brand Accent Color Picker -->
        <div>
          <label class="block text-xs font-medium text-slate-300 mb-2">Primary Brand Accent Color</label>
          <div class="flex items-center gap-3">
            <div 
              v-for="color in colorPresets"
              :key="color.hex"
              @click="form.accentColor = color.hex"
              class="w-8 h-8 rounded-full cursor-pointer border-2 transition-all flex items-center justify-center"
              :style="{ backgroundColor: color.hex }"
              :class="form.accentColor === color.hex ? 'border-white scale-110 shadow-lg' : 'border-transparent opacity-80 hover:opacity-100'"
            >
              <Icon v-if="form.accentColor === color.hex" name="check" size="14" class="text-white drop-shadow" />
            </div>
          </div>
          <span class="text-[11px] text-slate-400 mt-2 block">
            Selected: <span class="font-mono text-indigo-300">{{ form.accentColor }}</span> (Applies to client booking interface)
          </span>
        </div>

        <div>
          <label class="block text-xs font-medium text-slate-300 mb-1">Business Physical Address</label>
          <input
            v-model="form.address"
            type="text"
            class="w-full bg-slate-900 border border-slate-700 rounded-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
          />
        </div>
      </div>

      <!-- Financial & Transaction Pricing Configuration -->
      <div class="glass-card rounded-2xl p-6 border border-slate-800 space-y-4">
        <h4 class="text-sm font-bold text-white flex items-center gap-2">
          <Icon name="dollar-sign" size="16" class="text-emerald-400" />
          <span>Transaction Pricing & Payment Rules</span>
        </h4>

        <!-- Tax & Fee Parameters -->
        <div class="grid grid-cols-3 gap-3">
          <div>
            <label class="block text-xs font-medium text-slate-300 mb-1">Tax Rate (%)</label>
            <input
              v-model="form.settings.taxRatePercent"
              type="number"
              step="0.01"
              min="0"
              class="w-full bg-slate-900 border border-slate-700 rounded-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
            />
          </div>

          <div>
            <label class="block text-xs font-medium text-slate-300 mb-1">Deposit Rate (%)</label>
            <input
              v-model="form.settings.depositPercent"
              type="number"
              min="5"
              max="100"
              class="w-full bg-slate-900 border border-slate-700 rounded-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
            />
          </div>

          <div>
            <label class="block text-xs font-medium text-slate-300 mb-1">Processing Fee ($)</label>
            <input
              v-model="form.settings.processingFee"
              type="number"
              step="0.10"
              min="0"
              class="w-full bg-slate-900 border border-slate-700 rounded-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
            />
          </div>
        </div>

        <!-- Scheduling Rules -->
        <div class="grid grid-cols-2 gap-3 pt-2">
          <div>
            <label class="block text-xs font-medium text-slate-300 mb-1">Minimum Notice (Hours)</label>
            <input
              v-model="form.settings.bookingNoticeHours"
              type="number"
              min="0"
              class="w-full bg-slate-900 border border-slate-700 rounded-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
            />
          </div>

          <div>
            <label class="block text-xs font-medium text-slate-300 mb-1">Cancellation Cutoff (Hours)</label>
            <input
              v-model="form.settings.cancelWindowHours"
              type="number"
              min="0"
              class="w-full bg-slate-900 border border-slate-700 rounded-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
            />
          </div>
        </div>

        <!-- Toggles -->
        <div class="space-y-3 pt-2">
          <label class="flex items-center gap-3 p-3 rounded-xl bg-slate-900/60 border border-slate-800 cursor-pointer">
            <input
              v-model="form.settings.allowTipping"
              type="checkbox"
              class="rounded bg-slate-800 border-slate-700 text-indigo-600 focus:ring-0"
            />
            <div class="text-xs">
              <span class="font-bold text-white block">Enable Customer Gratuity / Tipping</span>
              <span class="text-slate-400 text-[11px]">Display 15%, 18%, 20% gratuity options on transaction checkout</span>
            </div>
          </label>

          <label class="flex items-center gap-3 p-3 rounded-xl bg-slate-900/60 border border-slate-800 cursor-pointer">
            <input
              v-model="form.settings.allowInstantConfirmation"
              type="checkbox"
              class="rounded bg-slate-800 border-slate-700 text-indigo-600 focus:ring-0"
            />
            <div class="text-xs">
              <span class="font-bold text-white block">Instant Auto-Confirmation</span>
              <span class="text-slate-400 text-[11px]">Automatically confirm customer bookings upon authorized card charge</span>
            </div>
          </label>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup>
import { reactive, watch } from 'vue'
import Icon from '../common/Icon.vue'
import { useSlotly } from '../../composables/useSlotly'

const { currentTenant, updateTenantSettings } = useSlotly()

const colorPresets = [
  { name: 'Violet', hex: '#8b5cf6' },
  { name: 'Electric Cyan', hex: '#06b6d4' },
  { name: 'Warm Amber', hex: '#f59e0b' },
  { name: 'Emerald Mint', hex: '#10b981' },
  { name: 'Hot Rose', hex: '#ec4899' },
  { name: 'Royal Blue', hex: '#3b82f6' }
]

const form = reactive({
  name: currentTenant.value.name,
  tagline: currentTenant.value.tagline,
  slug: currentTenant.value.slug,
  badge: currentTenant.value.badge,
  accentColor: currentTenant.value.accentColor,
  address: currentTenant.value.address,
  settings: { ...currentTenant.value.settings }
})

watch(currentTenant, (newT) => {
  if (newT) {
    form.name = newT.name
    form.tagline = newT.tagline
    form.slug = newT.slug
    form.badge = newT.badge
    form.accentColor = newT.accentColor
    form.address = newT.address
    form.settings = { ...newT.settings }
  }
})

function saveAllSettings() {
  updateTenantSettings({
    name: form.name,
    tagline: form.tagline,
    slug: form.slug,
    badge: form.badge,
    accentColor: form.accentColor,
    address: form.address,
    settings: {
      ...form.settings,
      taxRatePercent: Number(form.settings.taxRatePercent) || 0,
      depositPercent: Number(form.settings.depositPercent) || 20,
      processingFee: Number(form.settings.processingFee) || 0
    }
  })
}
</script>
