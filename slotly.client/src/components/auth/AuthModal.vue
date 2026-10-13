<template>
  <div v-if="isAuthModalOpen" class="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/70 backdrop-blur-sm">
    <div class="glass-card rounded-3xl max-w-md w-full p-6 sm:p-8 border border-slate-700/80 shadow-2xl relative">
      <!-- Close Button -->
      <button 
        @click="closeAuth"
        class="absolute top-5 right-5 text-slate-400 hover:text-white p-1 rounded-lg hover:bg-slate-800 transition-colors"
      >
        <Icon name="x" size="18" />
      </button>

      <!-- User Role Selector Pills: Client vs Tenant -->
      <div class="flex items-center bg-slate-900 border border-slate-800 p-1 rounded-xl mb-6 text-xs">
        <button
          @click="authModalTab = 'client'"
          class="flex-1 py-2 rounded-lg font-semibold transition-all flex items-center justify-center gap-1.5"
          :class="authModalTab === 'client' ? 'bg-indigo-600 text-white shadow-md' : 'text-slate-400 hover:text-white'"
        >
          <Icon name="user" size="14" />
          <span>Client Account</span>
        </button>
        <button
          @click="authModalTab = 'tenant'"
          class="flex-1 py-2 rounded-lg font-semibold transition-all flex items-center justify-center gap-1.5"
          :class="authModalTab === 'tenant' ? 'bg-indigo-600 text-white shadow-md' : 'text-slate-400 hover:text-white'"
        >
          <Icon name="building" size="14" />
          <span>Business Tenant</span>
        </button>
      </div>

      <!-- Title & Context -->
      <div class="text-center mb-6">
        <div 
          class="w-12 h-12 rounded-2xl mx-auto flex items-center justify-center text-white shadow-lg mb-3"
          :class="authModalTab === 'client' ? 'bg-indigo-600 shadow-indigo-600/25' : 'bg-emerald-600 shadow-emerald-600/25'"
        >
          <Icon :name="authModalTab === 'client' ? 'calendar' : 'building'" size="22" />
        </div>
        <h3 class="text-xl font-extrabold text-white">
          {{ authModalMode === 'login' ? 'Sign In to Slotly' : 'Create Your Account' }}
        </h3>
        <p class="text-xs text-slate-400 mt-1">
          {{ authModalTab === 'client' 
            ? 'Manage your bookings, reschedule, and view digital tickets' 
            : 'Access your tenant schedule, client roster, and custom portal' }}
        </p>
      </div>

      <!-- Quick 1-Click Demo Logins for Fast Evaluation -->
      <div class="mb-5 p-3 rounded-xl bg-slate-900/60 border border-slate-800">
        <span class="text-[10px] uppercase font-bold text-slate-400 tracking-wider block mb-2">
          ⚡ 1-Click Instant Demo Login:
        </span>
        <div class="grid grid-cols-2 gap-2 text-xs">
          <button
            type="button"
            @click="quickDemoLogin('client')"
            class="py-1.5 px-2.5 rounded-lg bg-slate-800 hover:bg-slate-700 text-slate-200 font-medium truncate text-center transition-colors"
          >
            Demo Client
          </button>
          <button
            type="button"
            @click="quickDemoLogin('tenant')"
            class="py-1.5 px-2.5 rounded-lg bg-indigo-950/60 hover:bg-indigo-900/60 text-indigo-300 border border-indigo-500/30 font-medium truncate text-center transition-colors"
          >
            Demo Business
          </button>
        </div>
      </div>

      <!-- Main Form (Minimal Input) -->
      <form @submit.prevent="handleSubmit" class="space-y-4">
        <!-- Register-only fields -->
        <div v-if="authModalMode === 'register'">
          <label class="block text-xs font-semibold text-slate-300 mb-1">
            {{ authModalTab === 'client' ? 'Full Name *' : 'Your Name *' }}
          </label>
          <input
            v-model="name"
            required
            type="text"
            :placeholder="authModalTab === 'client' ? 'e.g. Vivienne St. Claire' : 'e.g. Marcus Vance'"
            class="w-full bg-slate-900 border border-slate-700/80 rounded-xl px-3.5 py-2.5 text-xs text-white placeholder-slate-500 focus:outline-none focus:border-indigo-500"
          />
        </div>

        <div v-if="authModalMode === 'register' && authModalTab === 'tenant'">
          <label class="block text-xs font-semibold text-slate-300 mb-1">Business / Organization Name *</label>
          <input
            v-model="orgName"
            required
            type="text"
            placeholder="e.g. Grand Plaza Ballroom & Suites"
            class="w-full bg-slate-900 border border-slate-700/80 rounded-xl px-3.5 py-2.5 text-xs text-white placeholder-slate-500 focus:outline-none focus:border-indigo-500"
          />
        </div>

        <div>
          <label class="block text-xs font-semibold text-slate-300 mb-1">
            {{ authModalTab === 'tenant' ? 'Work Email *' : 'Email Address *' }}
          </label>
          <input
            v-model="email"
            required
            type="email"
            placeholder="name@example.com"
            class="w-full bg-slate-900 border border-slate-700/80 rounded-xl px-3.5 py-2.5 text-xs text-white placeholder-slate-500 focus:outline-none focus:border-indigo-500"
          />
        </div>

        <div>
          <label class="block text-xs font-semibold text-slate-300 mb-1">Password *</label>
          <input
            v-model="password"
            required
            type="password"
            placeholder="••••••••"
            class="w-full bg-slate-900 border border-slate-700/80 rounded-xl px-3.5 py-2.5 text-xs text-white placeholder-slate-500 focus:outline-none focus:border-indigo-500"
          />
        </div>

        <!-- Submit Button -->
        <button
          type="submit"
          class="w-full py-3 rounded-xl text-xs font-bold text-white bg-gradient-to-r from-indigo-600 to-purple-600 hover:from-indigo-500 hover:to-purple-500 shadow-xl shadow-indigo-600/30 transition-all"
        >
          {{ authModalMode === 'login' ? 'Sign In' : 'Create Free Account' }}
        </button>
      </form>

      <!-- Toggle between Login & Register -->
      <div class="mt-6 text-center text-xs text-slate-400">
        <span v-if="authModalMode === 'login'">
          Don't have an account yet? 
          <button @click="authModalMode = 'register'" class="font-bold text-indigo-400 hover:text-indigo-300 ml-1">
            Sign up in seconds
          </button>
        </span>
        <span v-else>
          Already have an account? 
          <button @click="authModalMode = 'login'" class="font-bold text-indigo-400 hover:text-indigo-300 ml-1">
            Sign in
          </button>
        </span>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref } from 'vue'
import Icon from '../common/Icon.vue'
import { useSlotly } from '../../composables/useSlotly'

const {
  isAuthModalOpen,
  authModalTab,
  authModalMode,
  closeAuth,
  loginUser,
  registerUser
} = useSlotly()

const name = ref('')
const orgName = ref('')
const email = ref('')
const password = ref('')

function handleSubmit() {
  if (authModalMode.value === 'login') {
    loginUser({
      email: email.value,
      password: password.value,
      role: authModalTab.value
    })
  } else {
    registerUser({
      name: name.value,
      orgName: orgName.value,
      email: email.value,
      password: password.value,
      role: authModalTab.value
    })
  }

  // Clear fields
  name.value = ''
  orgName.value = ''
  email.value = ''
  password.value = ''
}

function quickDemoLogin(role) {
  if (role === 'client') {
    loginUser({
      email: 'seraphina.w@gmail.com',
      password: 'password123',
      role: 'client'
    })
  } else {
    loginUser({
      email: 'concierge@aurawellness.com',
      password: 'password123',
      role: 'tenant',
      tenantId: 'tenant-aura'
    })
  }
}
</script>
