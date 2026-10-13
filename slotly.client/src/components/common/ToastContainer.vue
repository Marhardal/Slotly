<template>
  <div class="fixed bottom-6 right-6 z-50 flex flex-col gap-3 pointer-events-none max-w-sm w-full">
    <transition-group
      enter-active-class="transform ease-out duration-300 transition"
      enter-from-class="translate-y-2 opacity-0 sm:translate-y-0 sm:translate-x-2"
      enter-to-class="translate-y-0 opacity-100 sm:translate-x-0"
      leave-active-class="transition ease-in duration-200"
      leave-from-class="opacity-100"
      leave-to-class="opacity-0 scale-95"
    >
      <div
        v-for="toast in toasts"
        :key="toast.id"
        class="pointer-events-auto flex items-start gap-3 p-4 rounded-xl shadow-2xl border backdrop-blur-xl"
        :class="[
          toast.type === 'success' ? 'bg-slate-900/95 border-emerald-500/40 text-emerald-200' :
          toast.type === 'error' ? 'bg-slate-900/95 border-rose-500/40 text-rose-200' :
          'bg-slate-900/95 border-indigo-500/40 text-indigo-200'
        ]"
      >
        <div class="mt-0.5">
          <Icon
            :name="toast.type === 'success' ? 'check-circle' : toast.type === 'error' ? 'x' : 'bell'"
            size="18"
            :class="[
              toast.type === 'success' ? 'text-emerald-400' :
              toast.type === 'error' ? 'text-rose-400' :
              'text-indigo-400'
            ]"
          />
        </div>
        <div class="flex-1 min-w-0">
          <h4 class="text-xs font-semibold text-white">{{ toast.title }}</h4>
          <p class="text-xs text-slate-300 mt-0.5 leading-relaxed">{{ toast.message }}</p>
        </div>
        <button
          @click="removeToast(toast.id)"
          class="text-slate-400 hover:text-white transition-colors"
        >
          <Icon name="x" size="14" />
        </button>
      </div>
    </transition-group>
  </div>
</template>

<script setup>
import Icon from './Icon.vue'
import { useSlotly } from '../../composables/useSlotly'

const { toasts, removeToast } = useSlotly()
</script>
