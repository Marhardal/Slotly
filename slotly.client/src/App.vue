<template>
  <div class="min-h-screen flex flex-col font-sans selection:bg-indigo-500 selection:text-white transition-colors">
    <!-- Top Global Sticky Navbar with Multi-Tenant Bar -->
    <TopNavigation />

    <!-- Main Dynamic View Area -->
    <main class="flex-1">
      <transition name="fade" mode="out-in">
        <component :is="activeComponent" />
      </transition>
    </main>

    <!-- Global Floating Notifications -->
    <ToastContainer />

    <!-- Minimal Input Auth Modal for Clients & Tenants -->
    <AuthModal />
  </div>
</template>

<script setup>
import { computed } from 'vue'
import TopNavigation from './components/common/TopNavigation.vue'
import ToastContainer from './components/common/ToastContainer.vue'
import AuthModal from './components/auth/AuthModal.vue'
import LandingPage from './components/landing/LandingPage.vue'
import ClientBookingPortal from './components/booking/ClientBookingPortal.vue'
import TenantDashboard from './components/dashboard/TenantDashboard.vue'
import { useSlotly } from './composables/useSlotly'

const { currentView } = useSlotly()

const activeComponent = computed(() => {
  switch (currentView.value) {
    case 'client-portal':
      return ClientBookingPortal
    case 'tenant-dashboard':
      return TenantDashboard
    case 'landing':
    default:
      return LandingPage
  }
})
</script>

<style>
.fade-enter-active,
.fade-leave-active {
  transition: opacity 0.2s ease, transform 0.2s ease;
}

.fade-enter-from {
  opacity: 0;
  transform: translateY(4px);
}

.fade-leave-to {
  opacity: 0;
  transform: translateY(-4px);
}
</style>
