<template>
  <div class="relative overflow-hidden">
    <!-- Background Ambient Glow Orbs -->
    <div class="absolute top-20 left-1/2 -translate-x-1/2 w-[800px] h-[400px] bg-gradient-to-tr from-indigo-600/20 via-purple-600/15 to-transparent blur-[120px] pointer-events-none -z-10"></div>
    <div class="absolute top-[800px] right-0 w-[500px] h-[500px] bg-emerald-600/10 blur-[130px] pointer-events-none -z-10"></div>

    <!-- HERO SECTION -->
    <section class="pt-16 pb-20 px-4 sm:px-6 lg:px-8 max-w-7xl mx-auto text-center relative">
      <!-- Announcement Pill -->
      <div class="inline-flex items-center gap-2 px-3.5 py-1.5 rounded-full bg-slate-900/90 border border-indigo-500/30 text-xs font-semibold text-slate-200 shadow-xl mb-8">
        <span class="flex h-2 w-2 rounded-full bg-indigo-400 animate-pulse"></span>
        <span class="text-indigo-300 font-bold">Slotly Cloud</span>
        <span class="text-slate-500">•</span>
        <span>Engineered for Services, Hotels, Event Halls & Multi-Location Chains</span>
        <Icon name="arrow-right" size="13" class="text-indigo-400" />
      </div>

      <!-- Main Headline -->
      <h1 class="text-4xl sm:text-6xl lg:text-7xl font-extrabold tracking-tight text-white max-w-5xl mx-auto leading-[1.12]">
        The Multi-Tenant Booking Engine for 
        <span class="text-gradient-primary">Services, Hotels & Event Venues</span>
      </h1>

      <p class="mt-6 text-base sm:text-xl text-slate-300 max-w-3xl mx-auto leading-relaxed">
        Scale from a boutique spa or banquet hall to hundreds of white-labeled locations. Offer instant client self-scheduling, real-time transaction billing, and zero-conflict multi-calendar management.
      </p>

      <!-- CTA Buttons -->
      <div class="mt-10 flex flex-col sm:flex-row items-center justify-center gap-4">
        <button
          @click="setView('client-portal')"
          class="w-full sm:w-auto px-8 py-3.5 rounded-2xl text-sm font-bold text-white bg-gradient-to-r from-indigo-600 via-purple-600 to-indigo-600 hover:from-indigo-500 hover:to-purple-500 shadow-xl shadow-indigo-600/30 flex items-center justify-center gap-2 transform hover:-translate-y-0.5 transition-all"
        >
          <Icon name="calendar" size="18" />
          <span>Test Client Booking Experience</span>
          <Icon name="arrow-right" size="16" />
        </button>

        <button
          @click="openAuth('tenant', 'register')"
          class="w-full sm:w-auto px-8 py-3.5 rounded-2xl text-sm font-bold text-slate-200 bg-slate-900/90 hover:bg-slate-800 border border-slate-700/80 shadow-lg flex items-center justify-center gap-2 transition-all"
        >
          <Icon name="building" size="18" class="text-indigo-400" />
          <span>Register Business Tenant (Quick)</span>
        </button>
      </div>

      <!-- Platform Trust Stats -->
      <div class="mt-16 grid grid-cols-2 md:grid-cols-4 gap-4 max-w-4xl mx-auto">
        <div class="glass-card rounded-2xl p-4 text-center">
          <div class="text-2xl sm:text-3xl font-extrabold text-white">100%</div>
          <div class="text-xs text-slate-400 mt-1">Tenant Data Isolation</div>
        </div>
        <div class="glass-card rounded-2xl p-4 text-center">
          <div class="text-2xl sm:text-3xl font-extrabold text-white">&lt; 40s</div>
          <div class="text-xs text-slate-400 mt-1">Checkout & Booking Speed</div>
        </div>
        <div class="glass-card rounded-2xl p-4 text-center">
          <div class="text-2xl sm:text-3xl font-extrabold text-white">Hotels & Venues</div>
          <div class="text-xs text-slate-400 mt-1">Multi-day & Hourly Staging</div>
        </div>
        <div class="glass-card rounded-2xl p-4 text-center">
          <div class="text-2xl sm:text-3xl font-extrabold text-white">99.99%</div>
          <div class="text-xs text-slate-400 mt-1">High Availability SLA</div>
        </div>
      </div>
    </section>

    <!-- LIVE INTERACTIVE MULTI-TENANT SHOWCASE SECTION -->
    <section class="py-16 px-4 sm:px-6 lg:px-8 max-w-7xl mx-auto">
      <div class="glass-card rounded-3xl p-6 sm:p-10 border border-indigo-500/20 shadow-2xl relative">
        <div class="text-center max-w-2xl mx-auto mb-8">
          <span class="px-3 py-1 rounded-full text-xs font-bold uppercase tracking-wider bg-indigo-500/20 text-indigo-300 border border-indigo-500/30">
            Interactive Multi-Tenant Preview
          </span>
          <h2 class="text-2xl sm:text-4xl font-extrabold text-white mt-3">
            One Core Engine. Any Business Model.
          </h2>
          <p class="text-xs sm:text-sm text-slate-300 mt-2">
            Switch between hotels, banquet halls, physical therapists, and wellness clinics below:
          </p>
        </div>

        <!-- Tenant Switcher Buttons -->
        <div class="flex items-center justify-center flex-wrap gap-3 mb-8">
          <button
            v-for="tenant in tenants"
            :key="tenant.id"
            @click="switchTenant(tenant.id)"
            class="px-5 py-2.5 rounded-xl text-xs font-semibold flex items-center gap-2.5 transition-all border"
            :class="[
              tenant.id === currentTenantId
                ? 'active-tenant-pill bg-slate-800 text-white shadow-xl scale-105'
                : 'bg-slate-900/60 text-slate-400 hover:text-slate-200 border-slate-800'
            ]"
            :style="tenant.id === currentTenantId ? { borderColor: tenant.accentColor } : {}"
          >
            <span class="w-3 h-3 rounded-full" :style="{ backgroundColor: tenant.accentColor }"></span>
            <span>{{ tenant.name }}</span>
            <span class="text-[10px] text-slate-500 font-mono hidden sm:inline">({{ tenant.subdomain }})</span>
          </button>
        </div>

        <!-- Live Tenant Mock Window -->
        <div class="bg-slate-950/90 rounded-2xl border border-slate-800 overflow-hidden shadow-2xl">
          <!-- Window Titlebar -->
          <div class="px-4 py-3 bg-slate-900/90 border-b border-slate-800 flex items-center justify-between text-xs">
            <div class="flex items-center gap-2">
              <span class="w-3 h-3 rounded-full bg-rose-500/80"></span>
              <span class="w-3 h-3 rounded-full bg-amber-500/80"></span>
              <span class="w-3 h-3 rounded-full bg-emerald-500/80"></span>
              <span class="ml-3 font-mono text-[11px] text-slate-400">https://{{ currentTenant.subdomain }}/book</span>
            </div>
            <div class="flex items-center gap-2">
              <span class="px-2 py-0.5 rounded text-[10px] font-bold uppercase tracking-wider text-slate-300 bg-slate-800">
                SSL Secured
              </span>
            </div>
          </div>

          <!-- Window Content Preview -->
          <div class="p-6 sm:p-8">
            <div class="flex flex-col md:flex-row items-start justify-between gap-6 pb-6 border-b border-slate-800/80">
              <div class="flex items-center gap-4">
                <div 
                  class="w-14 h-14 rounded-2xl flex items-center justify-center text-white shadow-xl"
                  :style="{ backgroundColor: currentTenant.accentColor }"
                >
                  <Icon :name="currentTenant.logoIcon" size="28" />
                </div>
                <div>
                  <h3 class="text-xl font-bold text-white">{{ currentTenant.name }}</h3>
                  <p class="text-xs text-slate-400 mt-0.5">{{ currentTenant.tagline }}</p>
                  <div class="flex items-center gap-3 text-xs text-slate-400 mt-2">
                    <span>{{ currentTenant.address }}</span>
                    <span>•</span>
                    <span class="text-amber-400 font-medium">★ {{ currentTenant.stats.rating }}</span>
                  </div>
                </div>
              </div>

              <div class="flex items-center gap-2">
                <button
                  @click="setView('client-portal')"
                  class="px-4 py-2 rounded-xl text-xs font-semibold text-white transition-all shadow-md"
                  :style="{ backgroundColor: currentTenant.accentColor }"
                >
                  Open Booking Wizard
                </button>
              </div>
            </div>

            <!-- Preview 3 Services / Venues -->
            <div class="grid grid-cols-1 sm:grid-cols-3 gap-4 mt-6">
              <div
                v-for="service in currentTenant.services.slice(0, 3)"
                :key="service.id"
                class="p-4 rounded-xl bg-slate-900/60 border border-slate-800 flex flex-col justify-between"
              >
                <div>
                  <div class="text-[10px] uppercase font-bold text-slate-400">{{ service.category }}</div>
                  <div class="text-sm font-bold text-white mt-1">{{ service.name }}</div>
                  <p class="text-xs text-slate-400 mt-1 line-clamp-2">{{ service.description }}</p>
                </div>
                <div class="flex items-center justify-between mt-4 pt-3 border-t border-slate-800 text-xs">
                  <span class="text-slate-400">{{ service.durationMinutes >= 60 ? `${(service.durationMinutes/60).toFixed(1)}h` : `${service.durationMinutes}m` }}</span>
                  <span class="font-bold text-white">${{ service.price }} USD</span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>

    <!-- ARCHITECTURAL ADVANTAGES -->
    <section class="py-16 px-4 sm:px-6 lg:px-8 max-w-7xl mx-auto">
      <div class="text-center max-w-3xl mx-auto mb-16">
        <span class="text-xs font-bold uppercase tracking-wider text-indigo-400 bg-indigo-950/60 px-3 py-1 rounded-full border border-indigo-500/20">
          Core Capabilities
        </span>
        <h2 class="text-3xl sm:text-4xl font-extrabold text-white mt-3">
          Appointments, Hotel Stays, and Event Venues
        </h2>
        <p class="text-sm text-slate-400 mt-2">
          From 30-minute massage appointments to full-day banquet hall galas and 24-hour luxury hotel stays.
        </p>
      </div>

      <div class="grid grid-cols-1 md:grid-cols-3 gap-6">
        <!-- Feature 1 -->
        <div class="glass-card rounded-2xl p-6 border border-slate-800 hover:border-slate-700 transition-all">
          <div class="w-12 h-12 rounded-xl bg-indigo-600/20 text-indigo-400 flex items-center justify-center border border-indigo-500/30 mb-4">
            <Icon name="hotel" size="24" />
          </div>
          <h3 class="text-lg font-bold text-white">Halls, Suites & Venues</h3>
          <p class="text-xs text-slate-300 mt-2 leading-relaxed">
            Support space-based bookings with guest capacity limits, event types (galas, weddings, conferences), and customizable banquet setups.
          </p>
        </div>

        <!-- Feature 2 -->
        <div class="glass-card rounded-2xl p-6 border border-slate-800 hover:border-slate-700 transition-all">
          <div class="w-12 h-12 rounded-xl bg-cyan-600/20 text-cyan-400 flex items-center justify-center border border-cyan-500/30 mb-4">
            <Icon name="receipt" size="24" />
          </div>
          <h3 class="text-lg font-bold text-white">Dynamic Transaction Billing</h3>
          <p class="text-xs text-slate-300 mt-2 leading-relaxed">
            Real-time calculation of optional add-ons, promo discounts, local taxes, deposits, and gratuities computed per transaction.
          </p>
        </div>

        <!-- Feature 3 -->
        <div class="glass-card rounded-2xl p-6 border border-slate-800 hover:border-slate-700 transition-all">
          <div class="w-12 h-12 rounded-xl bg-emerald-600/20 text-emerald-400 flex items-center justify-center border border-emerald-500/30 mb-4">
            <Icon name="building" size="24" />
          </div>
          <h3 class="text-lg font-bold text-white">Isolated Tenant Architecture</h3>
          <p class="text-xs text-slate-300 mt-2 leading-relaxed">
            Every business gets dedicated custom domains, isolated schedules, custom color palettes, and independent Stripe merchant accounts.
          </p>
        </div>
      </div>
    </section>

    <!-- FOOTER -->
    <footer class="border-t border-slate-800/80 bg-slate-950/80 pt-12 pb-8 px-4 sm:px-6 lg:px-8 max-w-7xl mx-auto text-xs text-slate-400">
      <div class="flex flex-col sm:flex-row items-center justify-between gap-6 pb-8 border-b border-slate-800/80">
        <div class="flex items-center gap-3">
          <div class="w-8 h-8 rounded-lg bg-indigo-600 flex items-center justify-center text-white font-bold">
            <Icon name="calendar" size="18" />
          </div>
          <div>
            <span class="text-sm font-bold text-white">Slotly</span>
            <p class="text-[11px] text-slate-400">Next-Gen Multi-Tenant Booking for Services & Venues</p>
          </div>
        </div>

        <div class="flex items-center gap-6">
          <button @click="setView('landing')" class="hover:text-white transition-colors">Platform</button>
          <button @click="setView('client-portal')" class="hover:text-white transition-colors">Booking Wizard</button>
          <button @click="setView('tenant-dashboard')" class="hover:text-white transition-colors">Tenant Dashboard</button>
          <button @click="openAuth('client', 'login')" class="hover:text-white transition-colors">Sign In</button>
        </div>
      </div>

      <div class="pt-6 flex flex-col sm:flex-row items-center justify-between gap-4 text-[11px] text-slate-500">
        <span>© 2026 Slotly Cloud Inc. All rights reserved.</span>
        <div class="flex items-center gap-4">
          <span>Multi-Tenant Architecture</span>
          <span>•</span>
          <span>Light & Dark Mode Support</span>
          <span>•</span>
          <span>Venues & Services Ready</span>
        </div>
      </div>
    </footer>
  </div>
</template>

<script setup>
import Icon from '../common/Icon.vue'
import { useSlotly } from '../../composables/useSlotly'

const {
  tenants,
  currentTenantId,
  currentTenant,
  switchTenant,
  openAuth,
  setView
} = useSlotly()
</script>
