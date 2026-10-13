<template>
  <div class="min-h-screen pb-20 pt-6 px-4 sm:px-6 lg:px-8">
    <div class="max-w-4xl mx-auto">
      
      <!-- Tenant Branded Header Card (White-label styling) -->
      <div class="glass-card rounded-2xl p-6 sm:p-8 mb-8 relative overflow-hidden shadow-2xl">
        <!-- Ambient Tenant Glow -->
        <div 
          class="absolute -right-20 -top-20 w-80 h-80 rounded-full blur-3xl opacity-20 pointer-events-none"
          :style="{ backgroundColor: currentTenant.accentColor }"
        ></div>

        <div class="relative z-10 flex flex-col sm:flex-row items-start sm:items-center justify-between gap-6">
          <div class="flex items-start gap-4">
            <div 
              class="w-16 h-16 rounded-2xl flex items-center justify-center text-white shadow-xl flex-shrink-0"
              :style="{ backgroundColor: currentTenant.accentColor }"
            >
              <Icon :name="currentTenant.logoIcon || 'sparkles'" size="32" class="text-white" />
            </div>
            <div>
              <div class="flex items-center gap-2 flex-wrap">
                <h1 class="text-2xl sm:text-3xl font-extrabold text-white tracking-tight">{{ currentTenant.name }}</h1>
                <span class="px-2.5 py-0.5 rounded-full text-xs font-semibold bg-white/10 text-slate-200 border border-white/10">
                  {{ currentTenant.badge }}
                </span>
              </div>
              <p class="text-slate-400 text-sm mt-1 max-w-xl">{{ currentTenant.tagline }}</p>
              
              <div class="flex flex-wrap items-center gap-y-2 gap-x-4 mt-3 text-xs text-slate-400">
                <span class="flex items-center gap-1.5">
                  <Icon name="map-pin" size="13" class="text-slate-400" />
                  {{ currentTenant.address }}
                </span>
                <span class="flex items-center gap-1.5">
                  <Icon name="phone" size="13" class="text-slate-400" />
                  {{ currentTenant.phone }}
                </span>
                <span class="flex items-center gap-1 text-amber-400 font-medium">
                  <Icon name="star" size="13" class="text-amber-400 fill-amber-400" />
                  {{ currentTenant.stats.rating }} ({{ currentTenant.stats.bookingsMonth }} reviews)
                </span>
              </div>
            </div>
          </div>

          <!-- Subdomain & Verified Badge -->
          <div class="sm:text-right flex-shrink-0 bg-slate-900/60 border border-slate-800 p-3 rounded-xl">
            <div class="flex items-center gap-1.5 text-xs text-emerald-400 font-medium">
              <Icon name="shield" size="14" />
              <span>Verified Organization</span>
            </div>
            <div class="text-[11px] font-mono text-slate-400 mt-1">
              {{ currentTenant.subdomain }}
            </div>
          </div>
        </div>
      </div>

      <!-- SUCCESS CONFIRMATION STATE (WITH COMPLETE ITEMIZED TRANSACTION RECEIPT) -->
      <div v-if="confirmedBooking" class="glass-card rounded-2xl p-6 sm:p-10 text-center max-w-2xl mx-auto border-emerald-500/30">
        <div class="w-16 h-16 rounded-full bg-emerald-500/20 text-emerald-400 border border-emerald-500/40 flex items-center justify-center mx-auto mb-4 animate-bounce">
          <Icon name="check" size="32" />
        </div>
        <span class="px-3 py-1 rounded-full text-xs font-semibold bg-emerald-500/20 text-emerald-300 border border-emerald-500/30">
          Transaction Processed & Confirmed
        </span>
        <h2 class="text-2xl sm:text-3xl font-bold text-white mt-3">You're All Set!</h2>
        <p class="text-slate-300 text-sm mt-1 max-w-md mx-auto">
          An itemized digital tax invoice and booking pass have been dispatched to 
          <span class="text-white font-medium">{{ confirmedBooking.clientEmail }}</span>.
        </p>

        <!-- Digital Appointment Ticket Pass with Transaction Breakdown -->
        <div class="bg-slate-900/90 rounded-2xl border border-slate-800 p-6 my-8 text-left relative overflow-hidden shadow-2xl">
          <!-- Top Header -->
          <div class="border-b border-slate-800 pb-4 mb-4 flex justify-between items-start">
            <div>
              <div class="text-xs uppercase tracking-wider text-slate-400 font-semibold flex items-center gap-1.5">
                <Icon name="receipt" size="14" class="text-indigo-400" />
                <span>Verified Booking Pass</span>
              </div>
              <div class="text-lg font-bold text-white mt-0.5">{{ confirmedBooking.serviceName }}</div>
              <div class="text-xs text-slate-400 mt-0.5">With {{ confirmedBooking.staffName }}</div>
            </div>
            <div class="text-right">
              <span class="text-xs font-mono px-2.5 py-1 rounded bg-slate-800 text-indigo-300 font-semibold">
                {{ confirmedBooking.pricing?.transactionId || confirmedBooking.id }}
              </span>
              <div class="text-xs text-emerald-400 font-medium mt-1">Status: Confirmed</div>
            </div>
          </div>

          <!-- Schedule Grid -->
          <div class="grid grid-cols-2 sm:grid-cols-4 gap-4 text-xs pb-4 border-b border-slate-800/80">
            <div>
              <div class="text-slate-400">Date</div>
              <div class="font-semibold text-white mt-1">{{ formatDate(confirmedBooking.date) }}</div>
            </div>
            <div>
              <div class="text-slate-400">Time</div>
              <div class="font-semibold text-white mt-1">{{ confirmedBooking.time }}</div>
            </div>
            <div>
              <div class="text-slate-400">Duration</div>
              <div class="font-semibold text-white mt-1">{{ confirmedBooking.duration >= 60 ? `${(confirmedBooking.duration/60).toFixed(1)} Hours` : `${confirmedBooking.duration} Mins` }}</div>
            </div>
            <div>
              <div class="text-slate-400">Payment Option</div>
              <div class="font-semibold text-indigo-300 mt-1 capitalize">
                {{ confirmedBooking.pricing?.paymentOption === 'deposit' ? 'Deposit Paid' : confirmedBooking.pricing?.paymentOption === 'in_person' ? 'Pay In Person' : 'Paid In Full' }}
              </div>
            </div>
          </div>

          <!-- Itemized Financial Breakdown Ledger -->
          <div class="py-4 border-b border-slate-800/80 text-xs space-y-2">
            <div class="text-[11px] uppercase tracking-wider font-bold text-slate-400">Itemized Transaction Receipt</div>
            
            <div class="flex justify-between text-slate-300">
              <span>{{ confirmedBooking.serviceName }} (Base Fee)</span>
              <span>${{ Number(confirmedBooking.pricing?.basePrice || 0).toFixed(2) }}</span>
            </div>

            <div 
              v-for="addon in (confirmedBooking.pricing?.addons || [])" 
              :key="addon.name"
              class="flex justify-between text-slate-300 text-[11px]"
            >
              <span class="text-slate-400">+ {{ addon.name }}</span>
              <span>+${{ Number(addon.price).toFixed(2) }}</span>
            </div>

            <div v-if="confirmedBooking.pricing?.discount > 0" class="flex justify-between text-emerald-400 font-medium text-[11px]">
              <span>Coupon Discount ({{ confirmedBooking.pricing?.discountCode }})</span>
              <span>-${{ Number(confirmedBooking.pricing?.discount).toFixed(2) }}</span>
            </div>

            <div class="flex justify-between text-slate-400 text-[11px]">
              <span>Tax & Surcharges ({{ currentTenant.settings.taxRatePercent }}%)</span>
              <span>+${{ Number(confirmedBooking.pricing?.tax || 0).toFixed(2) }}</span>
            </div>

            <div class="flex justify-between text-slate-400 text-[11px]">
              <span>Processing Fee</span>
              <span>+${{ Number(confirmedBooking.pricing?.processingFee || 0).toFixed(2) }}</span>
            </div>

            <div v-if="confirmedBooking.pricing?.tip > 0" class="flex justify-between text-slate-300 text-[11px]">
              <span>Gratuity / Tip</span>
              <span>+${{ Number(confirmedBooking.pricing?.tip).toFixed(2) }}</span>
            </div>

            <div class="pt-2 flex justify-between font-bold text-white text-sm border-t border-slate-800">
              <span>Total Transaction Value</span>
              <span>${{ Number(confirmedBooking.pricing?.grandTotal || 0).toFixed(2) }} USD</span>
            </div>

            <div class="flex justify-between text-emerald-400 font-semibold pt-1">
              <span>Amount Charged Now ({{ confirmedBooking.pricing?.paymentMethod }})</span>
              <span>${{ Number(confirmedBooking.pricing?.depositAmount || 0).toFixed(2) }}</span>
            </div>

            <div v-if="confirmedBooking.pricing?.balanceDue > 0" class="flex justify-between text-amber-400 font-semibold">
              <span>Remaining Balance Due at Arrival</span>
              <span>${{ Number(confirmedBooking.pricing?.balanceDue).toFixed(2) }}</span>
            </div>
          </div>

          <div class="mt-4 flex items-center justify-between text-xs text-slate-400">
            <span class="flex items-center gap-1.5">
              <Icon name="map-pin" size="13" class="text-slate-400" />
              {{ currentTenant.address }}
            </span>
            <span class="font-mono text-[11px] text-slate-400">Ref: {{ confirmedBooking.id }}</span>
          </div>
        </div>

        <div class="flex flex-col sm:flex-row items-center justify-center gap-3">
          <button
            @click="resetBookingForm"
            class="w-full sm:w-auto px-6 py-2.5 rounded-xl text-xs font-semibold text-white bg-slate-800 hover:bg-slate-700 border border-slate-700 transition-colors"
          >
            Book Another Appointment / Space
          </button>
          
          <button
            @click="setView('tenant-dashboard')"
            class="w-full sm:w-auto px-6 py-2.5 rounded-xl text-xs font-semibold text-white bg-gradient-to-r from-indigo-600 to-purple-600 hover:from-indigo-500 hover:to-purple-500 shadow-lg shadow-indigo-600/30 transition-all flex items-center justify-center gap-2"
          >
            <span>View In Admin Calendar</span>
            <Icon name="arrow-right" size="14" />
          </button>
        </div>
      </div>

      <!-- MAIN BOOKING FLOW WIZARD -->
      <div v-else class="glass-card rounded-2xl p-6 sm:p-8 shadow-2xl">
        <!-- Step Progress Bar -->
        <div class="mb-8">
          <div class="flex items-center justify-between text-xs font-medium text-slate-400 mb-2">
            <span :class="{ 'text-white font-semibold': step === 1 }">1. Choose Service / Venue</span>
            <span :class="{ 'text-white font-semibold': step === 2 }">2. Specialist & Date</span>
            <span :class="{ 'text-white font-semibold': step === 3 }">3. Time Slot</span>
            <span :class="{ 'text-white font-semibold': step === 4 }">4. Details & Billing</span>
          </div>
          <div class="w-full h-1.5 bg-slate-800 rounded-full overflow-hidden">
            <div 
              class="h-full transition-all duration-300 rounded-full"
              :style="{ 
                width: `${(step / 4) * 100}%`,
                backgroundColor: currentTenant.accentColor 
              }"
            ></div>
          </div>
        </div>

        <!-- STEP 1: SERVICE SELECTION (BUTTON ON EACH SERVICE CARD + INSTANT SEARCH/FILTER) -->
        <div v-if="step === 1" class="space-y-6">
          <div class="flex flex-col sm:flex-row sm:items-center justify-between gap-4">
            <div>
              <h2 class="text-xl font-bold text-white">Select a Service, Space or Suite</h2>
              <p class="text-xs text-slate-400 mt-0.5">
                Click any offering below to immediately proceed with specialist, date & time selection.
              </p>
            </div>
            <span class="text-xs text-slate-400 font-mono">
              Showing {{ filteredServices.length }} of {{ currentServices.length }} items
            </span>
          </div>

          <!-- Instant Search & Category Filter Toolbar (Crucial for 100+ services) -->
          <div class="flex flex-col sm:flex-row gap-3 items-stretch sm:items-center justify-between">
            <!-- Search Input -->
            <div class="relative flex-1 max-w-md">
              <Icon name="search" size="15" class="absolute left-3.5 top-1/2 -translate-y-1/2 text-slate-400" />
              <input
                v-model="serviceSearchQuery"
                type="text"
                placeholder="Search services, ballrooms, suites, treatments..."
                class="w-full bg-slate-900 border border-slate-700/80 rounded-xl pl-9 pr-4 py-2 text-xs text-white placeholder-slate-500 focus:outline-none focus:border-indigo-500"
              />
            </div>

            <!-- Category Pills -->
            <div class="flex items-center gap-1.5 overflow-x-auto pb-1 text-xs">
              <button
                @click="selectedCategory = 'all'"
                class="px-3 py-1.5 rounded-xl font-medium transition-all whitespace-nowrap"
                :class="selectedCategory === 'all' ? 'bg-indigo-600 text-white font-semibold shadow' : 'bg-slate-900 border border-slate-800 text-slate-400 hover:text-white'"
              >
                All
              </button>
              <button
                v-for="cat in availableCategories"
                :key="cat"
                @click="selectedCategory = cat"
                class="px-3 py-1.5 rounded-xl font-medium transition-all whitespace-nowrap"
                :class="selectedCategory === cat ? 'bg-indigo-600 text-white font-semibold shadow' : 'bg-slate-900 border border-slate-800 text-slate-400 hover:text-white'"
              >
                {{ cat }}
              </button>
            </div>
          </div>

          <!-- Service Grid with Action Button directly on each card -->
          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div
              v-for="service in filteredServices"
              :key="service.id"
              class="rounded-xl p-5 border transition-all duration-200 relative group flex flex-col justify-between"
              :class="[
                selectedService?.id === service.id
                  ? 'bg-slate-800/95 border-indigo-500 shadow-xl shadow-indigo-500/20'
                  : 'bg-slate-900/50 border-slate-800 hover:border-slate-700 hover:bg-slate-800/40'
              ]"
            >
              <div v-if="service.popular" class="absolute top-3 right-3">
                <span class="px-2 py-0.5 rounded-full text-[10px] font-bold bg-amber-500/20 text-amber-300 border border-amber-500/30">
                  Featured
                </span>
              </div>

              <div>
                <span class="text-[10px] font-bold uppercase tracking-wider text-indigo-400 bg-indigo-950/60 px-2 py-0.5 rounded border border-indigo-500/20">
                  {{ service.category }}
                </span>
                <h3 class="text-base font-bold text-white mt-2 group-hover:text-indigo-300 transition-colors">
                  {{ service.name }}
                </h3>
                <p class="text-xs text-slate-300 mt-2 leading-relaxed">
                  {{ service.description }}
                </p>
              </div>

              <!-- Card Bottom: Duration, Price, and Direct Continue Button -->
              <div class="flex items-center justify-between pt-4 mt-4 border-t border-slate-800/80 gap-3">
                <div>
                  <div class="text-base font-extrabold text-white">${{ service.price }} <span class="text-[11px] font-normal text-slate-400">USD</span></div>
                  <div class="flex items-center gap-1 text-[11px] text-slate-400 mt-0.5">
                    <Icon name="clock" size="12" class="text-slate-400" />
                    <span>{{ service.durationMinutes >= 60 ? `${(service.durationMinutes/60).toFixed(1)}h` : `${service.durationMinutes}m` }}</span>
                  </div>
                </div>

                <!-- DIRECT SELECT & PROCEED BUTTON ON EACH SERVICE CARD -->
                <button
                  @click="chooseServiceAndProceed(service)"
                  class="px-4 py-2.5 rounded-xl text-xs font-bold text-white transition-all flex items-center gap-1.5 shadow-md transform hover:scale-[1.03] active:scale-[0.98]"
                  :style="{ backgroundColor: currentTenant.accentColor }"
                >
                  <span>{{ isVenueOrSpace(service) ? 'Reserve Space' : 'Select & Continue' }}</span>
                  <Icon name="arrow-right" size="13" />
                </button>
              </div>
            </div>

            <!-- Empty State for 100+ services search -->
            <div v-if="filteredServices.length === 0" class="col-span-full p-10 text-center text-slate-500 text-xs">
              No services found matching "{{ serviceSearchQuery }}". Try another keyword or reset category filter.
            </div>
          </div>
        </div>

        <!-- STEP 2: SPECIALIST & DATE PICKER -->
        <div v-else-if="step === 2" class="space-y-6">
          <div class="flex items-center justify-between">
            <div>
              <h2 class="text-xl font-bold text-white">Choose Specialist / Host & Date</h2>
              <p class="text-xs text-slate-400 mt-0.5">
                Booking: <span class="text-white font-medium">{{ selectedService?.name }}</span> (${{ selectedService?.price }})
              </p>
            </div>
            <button @click="step = 1" class="text-xs text-slate-400 hover:text-white transition-colors">
              ← Change Service
            </button>
          </div>

          <!-- Specialist / Host Selector -->
          <div>
            <label class="block text-xs font-semibold text-slate-300 uppercase tracking-wider mb-3">
              {{ isVenueOrSpace(selectedService) ? 'Select Event Coordinator / Concierge' : 'Select Practitioner / Specialist' }}
            </label>
            <div class="grid grid-cols-1 sm:grid-cols-3 gap-3">
              <!-- Any available option -->
              <div
                @click="selectedStaff = { id: 'any', name: 'First Available Specialist', role: 'Fastest Availability' }"
                class="cursor-pointer rounded-xl p-3.5 border transition-all flex items-center gap-3"
                :class="selectedStaff?.id === 'any' ? 'bg-indigo-950/40 border-indigo-500 shadow-md' : 'bg-slate-900/60 border-slate-800 hover:bg-slate-800/50'"
              >
                <div class="w-10 h-10 rounded-full bg-indigo-600/30 border border-indigo-500/40 flex items-center justify-center text-indigo-300">
                  <Icon name="users" size="18" />
                </div>
                <div class="min-w-0">
                  <div class="text-xs font-bold text-white truncate">Any Available</div>
                  <div class="text-[11px] text-slate-400">Fastest Availability</div>
                </div>
              </div>

              <!-- Dedicated Staff Members -->
              <div
                v-for="staff in currentStaff"
                :key="staff.id"
                @click="selectedStaff = staff"
                class="cursor-pointer rounded-xl p-3.5 border transition-all flex items-center gap-3"
                :class="selectedStaff?.id === staff.id ? 'bg-indigo-950/40 border-indigo-500 shadow-md' : 'bg-slate-900/60 border-slate-800 hover:bg-slate-800/50'"
              >
                <img :src="staff.avatar" :alt="staff.name" class="w-10 h-10 rounded-full object-cover border border-slate-700" />
                <div class="min-w-0">
                  <div class="text-xs font-bold text-white truncate">{{ staff.name }}</div>
                  <div class="text-[11px] text-slate-400 truncate">{{ staff.role }}</div>
                  <div class="flex items-center gap-1 text-[10px] text-amber-400 mt-0.5">
                    <Icon name="star" size="10" class="fill-amber-400" />
                    <span>{{ staff.rating }}</span>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <!-- Date Picker -->
          <div>
            <label class="block text-xs font-semibold text-slate-300 uppercase tracking-wider mb-3">
              Select Reservation Date
            </label>
            <div class="grid grid-cols-2 sm:grid-cols-4 md:grid-cols-7 gap-2">
              <button
                v-for="d in upcomingDates"
                :key="d.dateString"
                @click="selectedDate = d.dateString"
                type="button"
                class="p-3 rounded-xl border text-center transition-all flex flex-col items-center justify-center"
                :class="[
                  selectedDate === d.dateString
                    ? 'bg-indigo-600 text-white border-indigo-500 shadow-lg shadow-indigo-600/30 font-bold'
                    : 'bg-slate-900/60 border-slate-800 text-slate-300 hover:bg-slate-800/60'
                ]"
              >
                <span class="text-[11px] uppercase tracking-wider opacity-75">{{ d.dayName }}</span>
                <span class="text-base font-extrabold my-0.5">{{ d.dayNum }}</span>
                <span class="text-[10px] opacity-75">{{ d.monthName }}</span>
              </button>
            </div>
          </div>

          <div class="flex items-center justify-between pt-4 border-t border-slate-800">
            <button
              @click="step = 1"
              class="px-4 py-2 rounded-xl text-xs font-medium text-slate-400 hover:text-white"
            >
              Back
            </button>
            <button
              :disabled="!selectedStaff || !selectedDate"
              @click="step = 3"
              class="px-6 py-2.5 rounded-xl text-xs font-semibold text-white transition-all flex items-center gap-2"
              :class="selectedStaff && selectedDate ? 'bg-indigo-600 hover:bg-indigo-500 shadow-lg shadow-indigo-600/30' : 'bg-slate-800 text-slate-500 cursor-not-allowed'"
            >
              <span>Continue to Time Slots</span>
              <Icon name="arrow-right" size="14" />
            </button>
          </div>
        </div>

        <!-- STEP 3: REAL-TIME TIME SLOT SELECTION -->
        <div v-else-if="step === 3" class="space-y-6">
          <div class="flex items-center justify-between">
            <div>
              <h2 class="text-xl font-bold text-white">Select a Time Slot</h2>
              <p class="text-xs text-slate-400 mt-0.5">
                Available slots for {{ formatDate(selectedDate) }} with {{ selectedStaff?.name }}
              </p>
            </div>
            <button @click="step = 2" class="text-xs text-slate-400 hover:text-white transition-colors">
              ← Change Date
            </button>
          </div>

          <!-- Morning Slots -->
          <div>
            <div class="flex items-center gap-2 text-xs font-semibold text-slate-300 uppercase tracking-wider mb-2">
              <Icon name="clock" size="14" class="text-amber-400" />
              <span>Morning Sessions</span>
            </div>
            <div class="grid grid-cols-2 sm:grid-cols-4 gap-2.5">
              <button
                v-for="slot in ['09:00 AM', '10:00 AM', '11:15 AM']"
                :key="slot"
                @click="selectedTime = slot"
                class="py-2.5 px-3 rounded-xl border text-xs font-medium transition-all"
                :class="selectedTime === slot ? 'bg-indigo-600 border-indigo-500 text-white shadow-md' : 'bg-slate-900/60 border-slate-800 text-slate-200 hover:bg-slate-800'"
              >
                {{ slot }}
              </button>
            </div>
          </div>

          <!-- Afternoon Slots -->
          <div>
            <div class="flex items-center gap-2 text-xs font-semibold text-slate-300 uppercase tracking-wider mb-2">
              <Icon name="clock" size="14" class="text-cyan-400" />
              <span>Afternoon Sessions</span>
            </div>
            <div class="grid grid-cols-2 sm:grid-cols-4 gap-2.5">
              <button
                v-for="slot in ['01:00 PM', '02:15 PM', '03:30 PM']"
                :key="slot"
                @click="selectedTime = slot"
                class="py-2.5 px-3 rounded-xl border text-xs font-medium transition-all"
                :class="selectedTime === slot ? 'bg-indigo-600 border-indigo-500 text-white shadow-md' : 'bg-slate-900/60 border-slate-800 text-slate-200 hover:bg-slate-800'"
              >
                {{ slot }}
              </button>
            </div>
          </div>

          <!-- Evening Slots -->
          <div>
            <div class="flex items-center gap-2 text-xs font-semibold text-slate-300 uppercase tracking-wider mb-2">
              <Icon name="clock" size="14" class="text-purple-400" />
              <span>Evening Sessions / Galas</span>
            </div>
            <div class="grid grid-cols-2 sm:grid-cols-4 gap-2.5">
              <button
                v-for="slot in ['04:45 PM', '05:30 PM', '07:00 PM']"
                :key="slot"
                @click="selectedTime = slot"
                class="py-2.5 px-3 rounded-xl border text-xs font-medium transition-all"
                :class="selectedTime === slot ? 'bg-indigo-600 border-indigo-500 text-white shadow-md' : 'bg-slate-900/60 border-slate-800 text-slate-200 hover:bg-slate-800'"
              >
                {{ slot }}
              </button>
            </div>
          </div>

          <div class="flex items-center justify-between pt-4 border-t border-slate-800">
            <button
              @click="step = 2"
              class="px-4 py-2 rounded-xl text-xs font-medium text-slate-400 hover:text-white"
            >
              Back
            </button>
            <button
              :disabled="!selectedTime"
              @click="step = 4"
              class="px-6 py-2.5 rounded-xl text-xs font-semibold text-white transition-all flex items-center gap-2"
              :class="selectedTime ? 'bg-indigo-600 hover:bg-indigo-500 shadow-lg shadow-indigo-600/30' : 'bg-slate-800 text-slate-500 cursor-not-allowed'"
            >
              <span>Continue to Intake & Transaction Billing</span>
              <Icon name="arrow-right" size="14" />
            </button>
          </div>
        </div>

        <!-- STEP 4: CLIENT CONTACT INFO, HOTEL/HALL PARAMETERS & LIVE TRANSACTION PRICING ENGINE -->
        <div v-else-if="step === 4" class="space-y-6">
          <div class="flex items-center justify-between">
            <div>
              <h2 class="text-xl font-bold text-white">Review & Complete Transaction</h2>
              <p class="text-xs text-slate-400 mt-0.5">Customize add-ons, review transparent pricing, and choose payment terms</p>
            </div>
            <button @click="step = 3" class="text-xs text-slate-400 hover:text-white transition-colors">
              ← Change Time
            </button>
          </div>

          <!-- Appointment Summary Pill -->
          <div class="p-4 rounded-xl bg-slate-900/80 border border-slate-800 flex flex-col sm:flex-row sm:items-center justify-between gap-3 text-xs">
            <div>
              <span class="text-slate-400">Selected Reservation:</span>
              <div class="font-bold text-white mt-0.5">{{ selectedService?.name }}</div>
              <div class="text-slate-400 text-[11px]">With {{ selectedStaff?.name }} • {{ selectedService?.durationMinutes >= 60 ? `${(selectedService?.durationMinutes/60).toFixed(1)}h` : `${selectedService?.durationMinutes}m` }}</div>
            </div>
            <div class="sm:text-right">
              <span class="text-slate-400">Date & Time:</span>
              <div class="font-bold text-indigo-400 mt-0.5">{{ formatDate(selectedDate) }} at {{ selectedTime }}</div>
              <div class="text-emerald-400 font-semibold">Base: ${{ selectedService?.price }} USD</div>
            </div>
          </div>

          <!-- SPECIAL VENUE / HOTEL / HALL SPECIFIC CONFIGURATION -->
          <div v-if="isVenueOrSpace(selectedService)" class="p-4 rounded-xl bg-slate-900/70 border border-slate-800 space-y-3 text-xs">
            <span class="font-bold text-white uppercase tracking-wider flex items-center gap-1.5 text-xs">
              <Icon name="hotel" size="14" class="text-emerald-400" />
              <span>Event / Stay Specifications</span>
            </span>

            <div class="grid grid-cols-1 sm:grid-cols-3 gap-3">
              <div>
                <label class="block text-[11px] text-slate-400 mb-1">Expected Guests / Attendees</label>
                <select
                  v-model="venueGuests"
                  class="w-full bg-slate-900 border border-slate-700/80 rounded-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
                >
                  <option value="1-2">1 - 2 Guests (Private Suite)</option>
                  <option value="10-25">10 - 25 Guests (Boardroom / Salon)</option>
                  <option value="50-100">50 - 100 Guests (Medium Reception)</option>
                  <option value="150-350">150 - 350 Guests (Grand Gala / Ballroom)</option>
                </select>
              </div>

              <div>
                <label class="block text-[11px] text-slate-400 mb-1">Event Type / Purpose</label>
                <select
                  v-model="venueEventType"
                  class="w-full bg-slate-900 border border-slate-700/80 rounded-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
                >
                  <option value="Banquet / Gala">Banquet Gala Dinner</option>
                  <option value="Wedding / Ceremony">Wedding Ceremony & Reception</option>
                  <option value="Corporate Summit">Corporate Executive Summit</option>
                  <option value="Luxury Hotel Stay">Luxury Hotel Overnight Stay</option>
                  <option value="Private Film / Shoot">Private Production / Filming</option>
                </select>
              </div>

              <div>
                <label class="block text-[11px] text-slate-400 mb-1">Floor Plan & Seating Setup</label>
                <select
                  v-model="venueSetup"
                  class="w-full bg-slate-900 border border-slate-700/80 rounded-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
                >
                  <option value="Round Banquet Tables">Round Banquet Tables (10/table)</option>
                  <option value="Keynote Theater">Keynote Theater Rows</option>
                  <option value="Cocktail Lounge">Cocktail Standing Lounge</option>
                  <option value="Master King Suite">Executive King Suite Layout</option>
                </select>
              </div>
            </div>
          </div>

          <!-- SECTION: OPTIONAL UPGRADES & ADD-ONS -->
          <div v-if="currentAddons.length > 0" class="space-y-3">
            <div class="flex items-center justify-between">
              <span class="text-xs font-bold text-white uppercase tracking-wider flex items-center gap-1.5">
                <Icon name="sparkles" size="14" class="text-indigo-400" />
                <span>Customize Your Reservation (Optional Enhancements)</span>
              </span>
              <span class="text-[11px] text-slate-400">Adds to transaction total</span>
            </div>

            <div class="grid grid-cols-1 sm:grid-cols-2 gap-3">
              <div
                v-for="addon in currentAddons"
                :key="addon.id"
                @click="toggleAddon(addon)"
                class="cursor-pointer p-3.5 rounded-xl border text-xs transition-all flex items-start justify-between gap-3"
                :class="isAddonSelected(addon) ? 'bg-indigo-950/40 border-indigo-500 shadow-md' : 'bg-slate-900/50 border-slate-800 hover:bg-slate-800/40'"
              >
                <div class="flex items-start gap-2.5">
                  <div 
                    class="w-5 h-5 rounded-md flex items-center justify-center mt-0.5 border transition-colors"
                    :class="isAddonSelected(addon) ? 'bg-indigo-600 border-indigo-500 text-white' : 'border-slate-700 text-transparent'"
                  >
                    <Icon name="check" size="12" />
                  </div>
                  <div>
                    <div class="font-bold text-white">{{ addon.name }}</div>
                    <p class="text-[11px] text-slate-400 mt-0.5">{{ addon.description }}</p>
                  </div>
                </div>
                <div class="font-bold text-indigo-300 flex-shrink-0 text-right">
                  +${{ addon.price }}
                </div>
              </div>
            </div>
          </div>

          <!-- CLIENT INFORMATION FORM -->
          <form @submit.prevent="handleBookingSubmit" class="space-y-6">
            <div class="space-y-3">
              <span class="text-xs font-bold text-white uppercase tracking-wider block">Customer Contact Information</span>
              <div class="grid grid-cols-1 sm:grid-cols-2 gap-4">
                <div>
                  <label class="block text-xs font-semibold text-slate-300 mb-1">Full Name *</label>
                  <input
                    v-model="clientName"
                    required
                    type="text"
                    placeholder="e.g. Vivienne St. Claire"
                    class="w-full bg-slate-900/90 border border-slate-700/80 rounded-xl px-3.5 py-2.5 text-xs text-white placeholder-slate-500 focus:outline-none focus:border-indigo-500"
                  />
                </div>

                <div>
                  <label class="block text-xs font-semibold text-slate-300 mb-1">Email Address *</label>
                  <input
                    v-model="clientEmail"
                    required
                    type="email"
                    placeholder="e.g. vivienne@example.com"
                    class="w-full bg-slate-900/90 border border-slate-700/80 rounded-xl px-3.5 py-2.5 text-xs text-white placeholder-slate-500 focus:outline-none focus:border-indigo-500"
                  />
                </div>
              </div>

              <div>
                <label class="block text-xs font-semibold text-slate-300 mb-1">Phone Number (SMS reminders & pass) *</label>
                <input
                  v-model="clientPhone"
                  required
                  type="tel"
                  placeholder="e.g. +1 (310) 555-0199"
                  class="w-full bg-slate-900/90 border border-slate-700/80 rounded-xl px-3.5 py-2.5 text-xs text-white placeholder-slate-500 focus:outline-none focus:border-indigo-500"
                />
              </div>

              <div>
                <label class="block text-xs font-semibold text-slate-300 mb-1">Special Notes / Requests (Optional)</label>
                <input
                  v-model="clientNotes"
                  type="text"
                  placeholder="Special requests, arrival notes, or AV instructions..."
                  class="w-full bg-slate-900/90 border border-slate-700/80 rounded-xl px-3.5 py-2.5 text-xs text-white placeholder-slate-500 focus:outline-none focus:border-indigo-500"
                />
              </div>
            </div>

            <!-- PROMO CODE INPUT & GRATUITY SELECTOR -->
            <div class="grid grid-cols-1 sm:grid-cols-2 gap-4 pt-2">
              <!-- Promo / Coupon Code -->
              <div class="p-4 rounded-xl bg-slate-900/60 border border-slate-800">
                <label class="block text-xs font-semibold text-slate-300 mb-2 flex items-center gap-1.5">
                  <Icon name="tag" size="14" class="text-indigo-400" />
                  <span>Promo / Discount Voucher</span>
                </label>
                <div class="flex gap-2">
                  <input
                    v-model="couponInput"
                    type="text"
                    placeholder="Try SLOTLY15 or VENUE100"
                    class="flex-1 bg-slate-900 border border-slate-700 rounded-xl px-3 py-2 text-xs text-white placeholder-slate-500 uppercase focus:outline-none focus:border-indigo-500"
                  />
                  <button
                    type="button"
                    @click="applyCoupon"
                    class="px-4 py-2 rounded-xl text-xs font-semibold text-white bg-indigo-600 hover:bg-indigo-500 transition-colors"
                  >
                    Apply
                  </button>
                </div>
                <div v-if="appliedCoupon" class="mt-2 text-xs text-emerald-400 flex items-center justify-between">
                  <span>✓ Applied {{ appliedCoupon.code }} ({{ appliedCoupon.description }})</span>
                  <button type="button" @click="removeCoupon" class="text-slate-400 hover:text-rose-400">Remove</button>
                </div>
              </div>

              <!-- Gratuity / Tip Selector -->
              <div v-if="currentTenant.settings.allowTipping" class="p-4 rounded-xl bg-slate-900/60 border border-slate-800">
                <label class="block text-xs font-semibold text-slate-300 mb-2">
                  Add Host Gratuity / Service Tip
                </label>
                <div class="grid grid-cols-4 gap-1.5">
                  <button
                    v-for="tipOpt in [0, 15, 18, 20]"
                    :key="tipOpt"
                    type="button"
                    @click="tipPercent = tipOpt"
                    class="py-2 rounded-xl border text-xs font-semibold transition-all text-center"
                    :class="tipPercent === tipOpt ? 'bg-indigo-600 border-indigo-500 text-white shadow' : 'bg-slate-900 border-slate-800 text-slate-300 hover:bg-slate-800'"
                  >
                    {{ tipOpt === 0 ? 'No Tip' : `${tipOpt}%` }}
                  </button>
                </div>
                <div class="text-[11px] text-slate-400 mt-2 text-right">
                  Tip amount: <span class="font-bold text-white">${{ calculatedPricing.tip.toFixed(2) }}</span>
                </div>
              </div>
            </div>

            <!-- LIVE REAL-TIME ITEMIZED TRANSACTION LEDGER -->
            <div class="p-5 rounded-2xl bg-gradient-to-br from-slate-900/90 to-slate-950/90 border border-slate-800 shadow-xl space-y-2.5 text-xs">
              <div class="flex items-center justify-between pb-2 border-b border-slate-800">
                <span class="font-bold text-white text-sm flex items-center gap-1.5">
                  <Icon name="receipt" size="16" class="text-indigo-400" />
                  <span>Transaction Pricing Breakdown</span>
                </span>
                <span class="text-[11px] text-slate-400 font-mono">Currency: {{ currentTenant.settings.currency }}</span>
              </div>

              <!-- Line Items -->
              <div class="flex justify-between text-slate-300">
                <span>{{ selectedService?.name }} (Base Rate)</span>
                <span class="font-mono">${{ Number(calculatedPricing.basePrice).toFixed(2) }}</span>
              </div>

              <div 
                v-for="addon in selectedAddons" 
                :key="addon.id"
                class="flex justify-between text-slate-400 text-[11px]"
              >
                <span>+ {{ addon.name }}</span>
                <span class="font-mono text-slate-300">+${{ Number(addon.price).toFixed(2) }}</span>
              </div>

              <div v-if="calculatedPricing.discount > 0" class="flex justify-between text-emerald-400 font-medium">
                <span>Promo Discount ({{ appliedCoupon?.code }})</span>
                <span class="font-mono">-${{ Number(calculatedPricing.discount).toFixed(2) }}</span>
              </div>

              <div class="flex justify-between text-slate-400 text-[11px]">
                <span>State & Local Tax ({{ currentTenant.settings.taxRatePercent }}%)</span>
                <span class="font-mono text-slate-300">+${{ Number(calculatedPricing.tax).toFixed(2) }}</span>
              </div>

              <div class="flex justify-between text-slate-400 text-[11px]">
                <span>Slotly Secure Processing Fee</span>
                <span class="font-mono text-slate-300">+${{ Number(calculatedPricing.processingFee).toFixed(2) }}</span>
              </div>

              <div v-if="calculatedPricing.tip > 0" class="flex justify-between text-slate-300 text-[11px]">
                <span>Host / Staff Gratuity</span>
                <span class="font-mono">+${{ Number(calculatedPricing.tip).toFixed(2) }}</span>
              </div>

              <!-- Grand Total -->
              <div class="pt-3 border-t border-slate-800 flex justify-between items-center text-sm font-bold text-white">
                <span>Total Transaction Amount</span>
                <span class="text-base text-indigo-300 font-mono">${{ Number(calculatedPricing.grandTotal).toFixed(2) }} USD</span>
              </div>
            </div>

            <!-- PAYMENT OPTIONS / TERMS SELECTION -->
            <div class="space-y-3">
              <span class="text-xs font-bold text-white uppercase tracking-wider block">Choose Payment Terms</span>
              <div class="grid grid-cols-1 sm:grid-cols-3 gap-3">
                <!-- Option 1: Full Payment -->
                <div
                  @click="paymentOption = 'full'"
                  class="cursor-pointer p-3.5 rounded-xl border text-xs transition-all"
                  :class="paymentOption === 'full' ? 'bg-indigo-950/40 border-indigo-500 shadow-md' : 'bg-slate-900/60 border-slate-800 hover:bg-slate-800/50'"
                >
                  <div class="font-bold text-white">Pay in Full Today</div>
                  <div class="text-[11px] text-slate-400 mt-1">Instant complete checkout</div>
                  <div class="font-bold text-emerald-400 mt-2">${{ Number(calculatedPricing.grandTotal).toFixed(2) }}</div>
                </div>

                <!-- Option 2: Deposit -->
                <div
                  @click="paymentOption = 'deposit'"
                  class="cursor-pointer p-3.5 rounded-xl border text-xs transition-all"
                  :class="paymentOption === 'deposit' ? 'bg-indigo-950/40 border-indigo-500 shadow-md' : 'bg-slate-900/60 border-slate-800 hover:bg-slate-800/50'"
                >
                  <div class="font-bold text-white">Pay {{ currentTenant.settings.depositPercent }}% Deposit</div>
                  <div class="text-[11px] text-slate-400 mt-1">Balance due at arrival</div>
                  <div class="font-bold text-indigo-300 mt-2">
                    ${{ Number(calculatedPricing.depositAmount).toFixed(2) }}
                    <span class="text-[10px] text-slate-500 block">Due later: ${{ Number(calculatedPricing.balanceDue).toFixed(2) }}</span>
                  </div>
                </div>

                <!-- Option 3: Pay at Venue -->
                <div
                  @click="paymentOption = 'in_person'"
                  class="cursor-pointer p-3.5 rounded-xl border text-xs transition-all"
                  :class="paymentOption === 'in_person' ? 'bg-indigo-950/40 border-indigo-500 shadow-md' : 'bg-slate-900/60 border-slate-800 hover:bg-slate-800/50'"
                >
                  <div class="font-bold text-white">Pay at Venue / Check-In</div>
                  <div class="text-[11px] text-slate-400 mt-1">Card held to guarantee</div>
                  <div class="font-bold text-amber-400 mt-2">$0.00 today</div>
                </div>
              </div>
            </div>

            <!-- PAYMENT CARD INPUT DETAILS -->
            <div class="p-4 rounded-xl bg-slate-900/70 border border-slate-800 space-y-3">
              <div class="flex items-center justify-between">
                <span class="text-xs font-bold text-white flex items-center gap-1.5">
                  <Icon name="credit-card" size="14" class="text-indigo-400" />
                  <span>Card Information</span>
                </span>
                <button
                  type="button"
                  @click="fillTestCard"
                  class="text-[11px] font-semibold text-indigo-400 hover:text-indigo-300 bg-indigo-950/50 px-2 py-0.5 rounded border border-indigo-500/30"
                >
                  Autofill Test Card
                </button>
              </div>

              <div>
                <label class="block text-[11px] text-slate-400 mb-1">Cardholder Name</label>
                <input
                  v-model="cardholderName"
                  required
                  type="text"
                  placeholder="Vivienne St. Claire"
                  class="w-full bg-slate-900 border border-slate-700/80 rounded-xl px-3 py-2 text-xs text-white focus:outline-none focus:border-indigo-500"
                />
              </div>

              <div class="grid grid-cols-3 gap-2">
                <div class="col-span-2">
                  <label class="block text-[11px] text-slate-400 mb-1">Card Number</label>
                  <input
                    v-model="cardNumber"
                    required
                    type="text"
                    maxlength="19"
                    placeholder="4242 •••• •••• 4242"
                    class="w-full bg-slate-900 border border-slate-700/80 rounded-xl px-3 py-2 text-xs text-white font-mono focus:outline-none focus:border-indigo-500"
                  />
                </div>
                <div>
                  <label class="block text-[11px] text-slate-400 mb-1">Exp & CVC</label>
                  <div class="flex gap-1">
                    <input
                      v-model="cardExp"
                      required
                      type="text"
                      placeholder="MM/YY"
                      maxlength="5"
                      class="w-1/2 bg-slate-900 border border-slate-700/80 rounded-xl px-2 py-2 text-xs text-white font-mono text-center focus:outline-none focus:border-indigo-500"
                    />
                    <input
                      v-model="cardCvc"
                      required
                      type="text"
                      placeholder="CVC"
                      maxlength="4"
                      class="w-1/2 bg-slate-900 border border-slate-700/80 rounded-xl px-2 py-2 text-xs text-white font-mono text-center focus:outline-none focus:border-indigo-500"
                    />
                  </div>
                </div>
              </div>

              <div class="flex items-center gap-1.5 text-[11px] text-slate-400 pt-1">
                <Icon name="lock" size="12" class="text-emerald-400" />
                <span>256-bit SSL encrypted. Payment routed under tenant Stripe Connect merchant account.</span>
              </div>
            </div>

            <!-- Guarantee Policy -->
            <div class="p-3 rounded-lg bg-indigo-950/30 border border-indigo-500/20 text-xs text-indigo-300 flex items-start gap-2.5">
              <Icon name="shield" size="16" class="text-indigo-400 mt-0.5 flex-shrink-0" />
              <span>
                <strong>Flexible Policy:</strong> Free cancellations and rescheduling allowed up to {{ currentTenant.settings.cancelWindowHours }} hours prior to scheduled start.
              </span>
            </div>

            <div class="flex items-center justify-between pt-4 border-t border-slate-800">
              <button
                type="button"
                @click="step = 3"
                class="px-4 py-2 rounded-xl text-xs font-medium text-slate-400 hover:text-white"
              >
                Back
              </button>
              <button
                type="submit"
                :disabled="isSubmitting"
                class="px-8 py-3 rounded-xl text-xs font-bold text-white bg-gradient-to-r from-indigo-600 via-purple-600 to-indigo-600 hover:from-indigo-500 hover:to-purple-500 shadow-xl shadow-indigo-600/30 transition-all flex items-center gap-2"
              >
                <Icon v-if="isSubmitting" name="refresh" size="14" class="animate-spin" />
                <span>
                  {{ isSubmitting 
                    ? 'Processing Transaction...' 
                    : paymentOption === 'deposit'
                      ? `Pay Deposit $${calculatedPricing.depositAmount.toFixed(2)} & Reserve`
                      : paymentOption === 'in_person'
                        ? 'Reserve with Card on File ($0.00 today)'
                        : `Pay $${calculatedPricing.grandTotal.toFixed(2)} & Confirm`
                  }}
                </span>
              </button>
            </div>
          </form>
        </div>

      </div>

    </div>
  </div>
</template>

<script setup>
import { ref, computed } from 'vue'
import Icon from '../common/Icon.vue'
import { useSlotly } from '../../composables/useSlotly'

const {
  currentTenant,
  currentServices,
  currentStaff,
  currentAddons,
  promoCodes,
  createBooking,
  setView,
  showToast
} = useSlotly()

const step = ref(1)
const selectedService = ref(null)
const selectedStaff = ref(null)
const selectedDate = ref('2026-10-14')
const selectedTime = ref('10:00 AM')

// Step 1 Filtering
const serviceSearchQuery = ref('')
const selectedCategory = ref('all')

const availableCategories = computed(() => {
  const set = new Set()
  currentServices.value.forEach(s => {
    if (s.category) set.add(s.category)
  })
  return Array.from(set)
})

const filteredServices = computed(() => {
  return currentServices.value.filter(s => {
    if (selectedCategory.value !== 'all' && s.category !== selectedCategory.value) {
      return false
    }
    if (serviceSearchQuery.value.trim() !== '') {
      const q = serviceSearchQuery.value.toLowerCase()
      const matchName = s.name.toLowerCase().includes(q)
      const matchDesc = (s.description || '').toLowerCase().includes(q)
      const matchCat = (s.category || '').toLowerCase().includes(q)
      return matchName || matchDesc || matchCat
    }
    return true
  })
})

// DIRECT BUTTON ACTION ON SERVICE CARD: Select & Jump directly to Step 2
function chooseServiceAndProceed(service) {
  selectedService.value = service
  step.value = 2
  window.scrollTo({ top: 0, behavior: 'smooth' })
}

function isVenueOrSpace(service) {
  if (!service) return false
  const cat = (service.category || '').toLowerCase()
  const name = (service.name || '').toLowerCase()
  return cat.includes('hall') || cat.includes('hotel') || cat.includes('venue') || cat.includes('suite') || cat.includes('ballroom') ||
         name.includes('hall') || name.includes('hotel') || name.includes('suite') || name.includes('ballroom') || name.includes('pavilion')
}

// Venue & Hotel specifications in Step 4
const venueGuests = ref('50-100')
const venueEventType = ref('Banquet / Gala')
const venueSetup = ref('Round Banquet Tables')

// Addons & Pricing parameters
const selectedAddons = ref([])
const couponInput = ref('')
const appliedCoupon = ref(null)
const tipPercent = ref(18)
const paymentOption = ref('full') // 'full' | 'deposit' | 'in_person'

// Client details & card
const clientName = ref('')
const clientEmail = ref('')
const clientPhone = ref('')
const clientNotes = ref('')
const cardholderName = ref('Vivienne St. Claire')
const cardNumber = ref('4242 •••• •••• 4242')
const cardExp = ref('12/28')
const cardCvc = ref('123')

const isSubmitting = ref(false)
const confirmedBooking = ref(null)

// Addon toggle helper
function toggleAddon(addon) {
  const idx = selectedAddons.value.findIndex(a => a.id === addon.id)
  if (idx > -1) {
    selectedAddons.value.splice(idx, 1)
  } else {
    selectedAddons.value.push(addon)
  }
}

function isAddonSelected(addon) {
  return selectedAddons.value.some(a => a.id === addon.id)
}

function applyCoupon() {
  const code = couponInput.value.trim().toUpperCase()
  const found = promoCodes.find(p => p.code === code)
  if (found) {
    appliedCoupon.value = found
    showToast('Promo Code Applied!', `${found.code} applied: ${found.description}`, 'success')
  } else {
    showToast('Invalid Code', 'Coupon code not found or expired. Try SLOTLY15, WELCOME20 or VENUE100', 'error')
  }
}

function removeCoupon() {
  appliedCoupon.value = null
  couponInput.value = ''
  showToast('Coupon Removed', 'Discount removed from transaction total', 'info')
}

function fillTestCard() {
  cardholderName.value = clientName.value || 'Vivienne St. Claire'
  cardNumber.value = '4242 •••• •••• 4242'
  cardExp.value = '12/28'
  cardCvc.value = '882'
  showToast('Test Card Injected', 'Stripe mock test card credentials filled', 'info')
}

// REAL-TIME TRANSACTION PRICING ENGINE
const calculatedPricing = computed(() => {
  const basePrice = selectedService.value?.price || 0
  const addonsTotal = selectedAddons.value.reduce((acc, curr) => acc + (curr.price || 0), 0)
  const initialSubtotal = basePrice + addonsTotal

  // Discount calculation
  let discount = 0
  if (appliedCoupon.value) {
    if (appliedCoupon.value.type === 'percent') {
      discount = (initialSubtotal * appliedCoupon.value.value) / 100
    } else if (appliedCoupon.value.type === 'fixed') {
      discount = Math.min(appliedCoupon.value.value, initialSubtotal)
    }
  }

  const netSubtotal = Math.max(0, initialSubtotal - discount)
  
  // Tax
  const taxRate = (currentTenant.value?.settings?.taxRatePercent || 8.5) / 100
  const tax = netSubtotal * taxRate

  // Processing Fee
  const processingFee = currentTenant.value?.settings?.processingFee || 2.50

  // Gratuity
  const tipRate = (tipPercent.value || 0) / 100
  const tip = currentTenant.value?.settings?.allowTipping ? netSubtotal * tipRate : 0

  // Grand Total
  const grandTotal = netSubtotal + tax + processingFee + tip

  // Deposit calculation
  const depositRate = (currentTenant.value?.settings?.depositPercent || 25) / 100
  let depositAmount = grandTotal
  let balanceDue = 0

  if (paymentOption.value === 'deposit') {
    depositAmount = Number((grandTotal * depositRate).toFixed(2))
    balanceDue = Number((grandTotal - depositAmount).toFixed(2))
  } else if (paymentOption.value === 'in_person') {
    depositAmount = 0
    balanceDue = grandTotal
  }

  return {
    basePrice,
    addons: selectedAddons.value.map(a => ({ name: a.name, price: a.price })),
    addonsTotal,
    discount,
    discountCode: appliedCoupon.value?.code || null,
    subtotal: initialSubtotal,
    tax,
    processingFee,
    tip,
    grandTotal,
    paymentOption: paymentOption.value,
    depositAmount,
    balanceDue,
    paymentMethod: paymentOption.value === 'in_person' ? 'Pay at Venue' : `Visa ending in ${cardNumber.value.slice(-4) || '4242'}`
  }
})

// Generate next 7 dates starting from 2026-10-14
const upcomingDates = computed(() => {
  const days = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat']
  const months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
  const base = new Date('2026-10-14T00:00:00')
  const list = []

  for (let i = 0; i < 7; i++) {
    const d = new Date(base)
    d.setDate(base.getDate() + i)
    const dateString = d.toISOString().split('T')[0]
    list.push({
      dateString,
      dayName: days[d.getDay()],
      dayNum: d.getDate(),
      monthName: months[d.getMonth()]
    })
  }
  return list
})

function formatDate(dateStr) {
  if (!dateStr) return ''
  const parts = dateStr.split('-')
  if (parts.length !== 3) return dateStr
  const d = new Date(dateStr)
  return d.toLocaleDateString('en-US', { weekday: 'short', month: 'short', day: 'numeric', year: 'numeric' })
}

function handleBookingSubmit() {
  isSubmitting.value = true

  const fullNotes = [
    clientNotes.value,
    isVenueOrSpace(selectedService.value) ? `[Venue: ${venueGuests.value} guests, Event: ${venueEventType.value}, Setup: ${venueSetup.value}]` : null
  ].filter(Boolean).join(' | ')

  setTimeout(() => {
    const booking = createBooking({
      clientName: clientName.value,
      clientEmail: clientEmail.value,
      clientPhone: clientPhone.value,
      serviceId: selectedService.value?.id,
      serviceName: selectedService.value?.name,
      staffId: selectedStaff.value?.id === 'any' ? (currentStaff.value[0]?.id || 'stf-auto') : selectedStaff.value?.id,
      staffName: selectedStaff.value?.name,
      date: selectedDate.value,
      time: selectedTime.value,
      duration: selectedService.value?.durationMinutes,
      price: calculatedPricing.value.grandTotal,
      pricing: { ...calculatedPricing.value, transactionId: `TXN-${Math.floor(10000 + Math.random() * 90000)}` },
      notes: fullNotes
    })

    confirmedBooking.value = booking
    isSubmitting.value = false
  }, 650)
}

function resetBookingForm() {
  confirmedBooking.value = null
  step.value = 1
  selectedService.value = null
  selectedStaff.value = null
  selectedAddons.value = []
  appliedCoupon.value = null
  couponInput.value = ''
  clientName.value = ''
  clientEmail.value = ''
  clientPhone.value = ''
  clientNotes.value = ''
}
</script>
