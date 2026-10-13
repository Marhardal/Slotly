// Initial multi-tenant dataset for Slotly with real-time transaction pricing models and hotel/hall venues

export const promoCodes = [
  { code: 'SLOTLY15', type: 'percent', value: 15, description: '15% off transaction total' },
  { code: 'WELCOME20', type: 'fixed', value: 20, description: '$20 off first appointment' },
  { code: 'VIP30', type: 'fixed', value: 30, description: '$30 off premium treatments' },
  { code: 'VENUE100', type: 'fixed', value: 100, description: '$100 off grand hall or suite bookings' }
]

export const initialTenants = [
  {
    id: 'tenant-aura',
    name: 'Aura Wellness & Spa',
    slug: 'aura-wellness',
    subdomain: 'aura.slotly.io',
    tagline: 'Holistic Bodywork, Hydrotherapy & Facial Rituals',
    category: 'Wellness & Aesthetics',
    badge: 'Luxury Wellness',
    accentColor: '#8b5cf6', // Violet
    accentGradient: 'from-violet-500 to-purple-600',
    logoIcon: 'sparkles',
    address: '450 Beverly Blvd, Suite 200, Los Angeles, CA',
    phone: '+1 (310) 882-9011',
    email: 'concierge@aurawellness.com',
    stats: {
      bookingsMonth: 184,
      revenueMonth: 23940,
      occupancyRate: 91,
      rating: 4.96,
      retentionRate: 88
    },
    settings: {
      bookingNoticeHours: 4,
      cancelWindowHours: 24,
      allowInstantConfirmation: true,
      requireDeposit: false,
      depositPercent: 25,
      taxRatePercent: 8.5,
      processingFee: 2.50,
      allowTipping: true,
      currency: 'USD'
    },
    addons: [
      {
        id: 'addon-aura-1',
        name: 'Organic Scalp Acupressure Treatment',
        price: 25,
        durationMinutes: 15,
        description: 'Warm botanical cedar & rosemary oil scalp massage.'
      },
      {
        id: 'addon-aura-2',
        name: 'Volcanic Basalt Hot Stones',
        price: 35,
        durationMinutes: 15,
        description: 'Heated river stones to deepen neuromuscular relaxation.'
      },
      {
        id: 'addon-aura-3',
        name: 'Collagen Peptide Eye Infusion',
        price: 20,
        durationMinutes: 0,
        description: 'Intense micro-current hydration mask for under-eye brightening.'
      }
    ],
    services: [
      {
        id: 'srv-aura-1',
        name: 'Deep Tissue & Botanical Aromatherapy',
        category: 'Massage & Bodywork',
        durationMinutes: 75,
        price: 140,
        description: 'Therapeutic muscle tension release using warm organic jojoba infused with wild lavender & eucalyptus essences.',
        popular: true,
        icon: 'sparkles'
      },
      {
        id: 'srv-aura-2',
        name: 'HydraGlow Peptide Cellular Facial',
        category: 'Facial Aesthetics',
        durationMinutes: 60,
        price: 165,
        description: 'Deep pore vacuum extraction paired with lymphatic drainage and medical-grade hyaluronic peptide infusion.',
        popular: true,
        icon: 'sparkles'
      },
      {
        id: 'srv-aura-3',
        name: 'Infrared Detox Sauna & Contrast Plunge',
        category: 'Recovery',
        durationMinutes: 45,
        price: 85,
        description: 'Full-spectrum infrared radiant heating coupled with 48°F purified mineral cold water immersion.',
        popular: false,
        icon: 'refresh'
      },
      {
        id: 'srv-aura-4',
        name: 'Couples Sound Bath & Restorative Ritual',
        category: 'Holistic Ceremonies',
        durationMinutes: 90,
        price: 220,
        description: 'Tibetan singing bowl resonance, grounding breathwork, and synchronistic pressure point therapy for two.',
        popular: false,
        icon: 'users'
      }
    ],
    staff: [
      {
        id: 'stf-aura-1',
        name: 'Elena Rostova',
        role: 'Master Aesthetician & Skin Director',
        rating: 4.98,
        avatar: 'https://images.unsplash.com/photo-1573496359142-b8d87734a5a2?w=150&auto=format&fit=crop&q=80',
        availableDays: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri'],
        specialties: ['Facial Aesthetics', 'Lymphatic Drainage']
      },
      {
        id: 'stf-aura-2',
        name: 'Marcus Vance',
        role: 'Licensed Neuromuscular Therapist',
        rating: 4.94,
        avatar: 'https://images.unsplash.com/photo-1534528741775-53994a69daeb?w=150&auto=format&fit=crop&q=80',
        availableDays: ['Tue', 'Wed', 'Thu', 'Sat', 'Sun'],
        specialties: ['Deep Tissue', 'Trigger Point Therapy']
      },
      {
        id: 'stf-aura-3',
        name: 'Chloe Chen',
        role: 'Holistic Sound Practitioner',
        rating: 4.96,
        avatar: 'https://images.unsplash.com/photo-1580489944761-15a19d654956?w=150&auto=format&fit=crop&q=80',
        availableDays: ['Wed', 'Thu', 'Fri', 'Sat'],
        specialties: ['Sound Bath', 'Breathwork']
      }
    ],
    bookings: [
      {
        id: 'bk-1001',
        clientName: 'Seraphina Wright',
        clientEmail: 'seraphina.w@gmail.com',
        clientPhone: '+1 (310) 441-2993',
        serviceId: 'srv-aura-2',
        serviceName: 'HydraGlow Peptide Cellular Facial',
        staffId: 'stf-aura-1',
        staffName: 'Elena Rostova',
        date: '2026-10-14',
        time: '10:00 AM',
        duration: 60,
        price: 165,
        status: 'confirmed',
        notes: 'First time visiting. Sensitive skin around cheeks.',
        createdAt: '2026-10-12T10:15:00Z',
        pricing: {
          basePrice: 165,
          addons: [{ name: 'Collagen Peptide Eye Infusion', price: 20 }],
          addonsTotal: 20,
          discount: 0,
          discountCode: null,
          subtotal: 185,
          tax: 15.73,
          processingFee: 2.50,
          tip: 25.00,
          grandTotal: 228.23,
          paymentOption: 'full',
          depositAmount: 228.23,
          balanceDue: 0,
          paymentMethod: 'Visa •••• 4242',
          transactionId: 'TXN-91823'
        }
      },
      {
        id: 'bk-1002',
        clientName: 'Julian Croft',
        clientEmail: 'jcroft@venturecap.io',
        clientPhone: '+1 (415) 309-8812',
        serviceId: 'srv-aura-1',
        serviceName: 'Deep Tissue & Botanical Aromatherapy',
        staffId: 'stf-aura-2',
        staffName: 'Marcus Vance',
        date: '2026-10-14',
        time: '11:30 AM',
        duration: 75,
        price: 140,
        status: 'confirmed',
        notes: 'Focus on upper thoracic spine and neck stiffness.',
        createdAt: '2026-10-11T16:22:00Z',
        pricing: {
          basePrice: 140,
          addons: [{ name: 'Volcanic Basalt Hot Stones', price: 35 }],
          addonsTotal: 35,
          discount: 20,
          discountCode: 'WELCOME20',
          subtotal: 155,
          tax: 13.18,
          processingFee: 2.50,
          tip: 20.00,
          grandTotal: 190.68,
          paymentOption: 'deposit',
          depositAmount: 47.67,
          balanceDue: 143.01,
          paymentMethod: 'Mastercard •••• 8821',
          transactionId: 'TXN-91845'
        }
      }
    ]
  },
  {
    id: 'tenant-luminary',
    name: 'The Grand Luminary Hotel & Event Halls',
    slug: 'grand-luminary',
    subdomain: 'luminary.slotly.io',
    tagline: 'Grand Ballrooms, Luxury Hotel Suites & Executive Conference Halls',
    category: 'Hotels, Halls & Venues',
    badge: 'Hospitality & Venues',
    accentColor: '#10b981', // Emerald
    accentGradient: 'from-emerald-500 to-teal-600',
    logoIcon: 'hotel',
    address: '700 Grand Plaza Boulevard, New York, NY',
    phone: '+1 (212) 880-9200',
    email: 'events@grandluminary.com',
    stats: {
      bookingsMonth: 64,
      revenueMonth: 82400,
      occupancyRate: 89,
      rating: 4.98,
      retentionRate: 91
    },
    settings: {
      bookingNoticeHours: 48,
      cancelWindowHours: 72,
      allowInstantConfirmation: false,
      requireDeposit: true,
      depositPercent: 30,
      taxRatePercent: 8.875,
      processingFee: 15.00,
      allowTipping: true,
      currency: 'USD'
    },
    addons: [
      {
        id: 'addon-lum-1',
        name: 'Concert-Grade Live AV & 4K Staging Production',
        price: 350,
        durationMinutes: 0,
        description: 'Dual 4K laser projectors, directional wireless microphones, and dedicated technician.'
      },
      {
        id: 'addon-lum-2',
        name: 'Executive Banquet White-Glove Hors d\'oeuvres Service',
        price: 500,
        durationMinutes: 0,
        description: 'Gourmet artisanal canapes and complimentary champagne tower toast.'
      },
      {
        id: 'addon-lum-3',
        name: 'Private Valet Parking Fleet Logistics',
        price: 250,
        durationMinutes: 0,
        description: 'Dedicated valet attendants managing up to 80 private vehicles.'
      },
      {
        id: 'addon-lum-4',
        name: 'Extended Hall Buffer (2 Additional Hours)',
        price: 400,
        durationMinutes: 120,
        description: 'Late teardown or midnight reception curfew extension.'
      }
    ],
    services: [
      {
        id: 'srv-lum-1',
        name: 'Grand Crystal Ballroom & Banquet Hall',
        category: 'Banquet & Gala Halls',
        durationMinutes: 360,
        price: 2400,
        description: 'Capacity up to 350 guests. Austrian crystal chandeliers, polished herringbone hardwood, and private bridal/VIP suite.',
        popular: true,
        icon: 'building'
      },
      {
        id: 'srv-lum-2',
        name: 'Skyline Penthouse Terrace Hotel Suite',
        category: 'Hotel Luxury Stays',
        durationMinutes: 1440,
        price: 850,
        description: '24-hour luxury stay. Panoramic city skyline views, private wraparound terrace, king suite, marble jacuzzi, and personal concierge.',
        popular: true,
        icon: 'hotel'
      },
      {
        id: 'srv-lum-3',
        name: 'Summit Executive Boardroom & Media Hall',
        category: 'Corporate Venues',
        durationMinutes: 240,
        price: 450,
        description: 'Capacity 28 seats. 130-inch interactive LED wall, studio acoustics, and barista beverage bar.',
        popular: false,
        icon: 'layers'
      },
      {
        id: 'srv-lum-4',
        name: 'The Glass Pavilion & Botanical Courtyard',
        category: 'Weddings & Receptions',
        durationMinutes: 480,
        price: 1800,
        description: 'Enclosed conservatory with transparent glass ceiling, perimeter gardens, and outdoor fire lounges.',
        popular: false,
        icon: 'sparkles'
      }
    ],
    staff: [
      {
        id: 'stf-lum-1',
        name: 'Genevieve Sterling',
        role: 'Director of Private Events & Galas',
        rating: 4.99,
        avatar: 'https://images.unsplash.com/photo-1573496359142-b8d87734a5a2?w=150&auto=format&fit=crop&q=80',
        availableDays: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
        specialties: ['Banquet Planning', 'VIP Receptions']
      },
      {
        id: 'stf-lum-2',
        name: 'Julian Beaumont',
        role: 'Head of Venue Staging & Audio Engineering',
        rating: 4.97,
        avatar: 'https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=150&auto=format&fit=crop&q=80',
        availableDays: ['Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
        specialties: ['AV Staging', 'Room Acoustics']
      }
    ],
    bookings: [
      {
        id: 'bk-4001',
        clientName: 'Alexander Montgomery',
        clientEmail: 'amontgomery@clarionwealth.com',
        clientPhone: '+1 (212) 555-9011',
        serviceId: 'srv-lum-1',
        serviceName: 'Grand Crystal Ballroom & Banquet Hall',
        staffId: 'stf-lum-1',
        staffName: 'Genevieve Sterling',
        date: '2026-10-17',
        time: '04:00 PM',
        duration: 360,
        price: 2400,
        status: 'confirmed',
        notes: 'Annual Investment Gala. Seating for 240 guests.',
        createdAt: '2026-10-10T14:00:00Z',
        pricing: {
          basePrice: 2400,
          addons: [
            { name: 'Concert-Grade Live AV & 4K Staging Production', price: 350 },
            { name: 'Executive Banquet White-Glove Hors d\'oeuvres Service', price: 500 }
          ],
          addonsTotal: 850,
          discount: 100,
          discountCode: 'VENUE100',
          subtotal: 3150,
          tax: 279.56,
          processingFee: 15.00,
          tip: 200.00,
          grandTotal: 3644.56,
          paymentOption: 'deposit',
          depositAmount: 1093.37,
          balanceDue: 2551.19,
          paymentMethod: 'Corporate Amex •••• 9921',
          transactionId: 'TXN-93102'
        }
      }
    ]
  },
  {
    id: 'tenant-apex',
    name: 'Apex Athletics & Physio',
    slug: 'apex-athletics',
    subdomain: 'apex.slotly.io',
    tagline: 'Performance Rehab, Biomechanics & Elite Conditioning',
    category: 'Sports Medicine & Fitness',
    badge: 'Clinical Performance',
    accentColor: '#06b6d4', // Cyan
    accentGradient: 'from-cyan-500 to-blue-600',
    logoIcon: 'zap',
    address: '120 Olympic Way, Suite 4B, Austin, TX',
    phone: '+1 (512) 640-3320',
    email: 'reception@apexphysio.com',
    stats: {
      bookingsMonth: 242,
      revenueMonth: 31200,
      occupancyRate: 94,
      rating: 4.92,
      retentionRate: 92
    },
    settings: {
      bookingNoticeHours: 2,
      cancelWindowHours: 12,
      allowInstantConfirmation: true,
      requireDeposit: true,
      depositPercent: 20,
      taxRatePercent: 8.25,
      processingFee: 2.00,
      allowTipping: false,
      currency: 'USD'
    },
    addons: [
      {
        id: 'addon-apex-1',
        name: 'Normatec Dynamic Air Compression (20m)',
        price: 30,
        durationMinutes: 20,
        description: 'Pulsed pneumatic compression for rapid lactic clearance.'
      },
      {
        id: 'addon-apex-2',
        name: 'Percussive Theragun Spot Release',
        price: 20,
        durationMinutes: 10,
        description: 'High-amplitude localized percussion for fascial knots.'
      }
    ],
    services: [
      {
        id: 'srv-apex-1',
        name: 'Musculoskeletal Assessment & Rehab Protocol',
        category: 'Physical Therapy',
        durationMinutes: 60,
        price: 150,
        description: 'Comprehensive functional mobility screening, joint articulation exam, and individualized recovery prescription.',
        popular: true,
        icon: 'shield'
      },
      {
        id: 'srv-apex-2',
        name: '3D High-Speed Running Gait Analysis',
        category: 'Biomechanics',
        durationMinutes: 45,
        price: 120,
        description: 'Motion tracking camera array capturing foot strike velocity, cadence asymmetry, and ground reaction forces.',
        popular: true,
        icon: 'bar-chart'
      },
      {
        id: 'srv-apex-3',
        name: 'Targeted Dry Needling & Myofascial Release',
        category: 'Therapeutic Modalities',
        durationMinutes: 45,
        price: 95,
        description: 'Filiform needle insertion into motor trigger bands to alleviate acute neuromuscular spasms.',
        popular: false,
        icon: 'zap'
      }
    ],
    staff: [
      {
        id: 'stf-apex-1',
        name: 'Dr. Tyler Hayes, DPT',
        role: 'Doctor of Physical Therapy & Clinic Director',
        rating: 4.96,
        avatar: 'https://images.unsplash.com/photo-1507003211169-0a1dd7228f2d?w=150&auto=format&fit=crop&q=80',
        availableDays: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri'],
        specialties: ['Spine & Knee Rehab', 'Dry Needling']
      },
      {
        id: 'stf-apex-2',
        name: 'Sofia Morales, CSCS',
        role: 'Lead Biomechanics & Strength Specialist',
        rating: 4.91,
        avatar: 'https://images.unsplash.com/photo-1544005313-94ddf0286df2?w=150&auto=format&fit=crop&q=80',
        availableDays: ['Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
        specialties: ['Running Mechanics', 'Return to Sport']
      }
    ],
    bookings: [
      {
        id: 'bk-2001',
        clientName: 'Brandon Cooper',
        clientEmail: 'b.cooper@triathlete.com',
        clientPhone: '+1 (512) 883-2001',
        serviceId: 'srv-apex-2',
        serviceName: '3D High-Speed Running Gait Analysis',
        staffId: 'stf-apex-2',
        staffName: 'Sofia Morales, CSCS',
        date: '2026-10-14',
        time: '09:30 AM',
        duration: 45,
        price: 120,
        status: 'confirmed',
        notes: 'Preparing for Austin Marathon. Right Achilles tightness.',
        createdAt: '2026-10-11T09:00:00Z',
        pricing: {
          basePrice: 120,
          addons: [{ name: 'Normatec Dynamic Air Compression (20m)', price: 30 }],
          addonsTotal: 30,
          discount: 0,
          discountCode: null,
          subtotal: 150,
          tax: 12.38,
          processingFee: 2.00,
          tip: 0,
          grandTotal: 164.38,
          paymentOption: 'deposit',
          depositAmount: 32.88,
          balanceDue: 131.50,
          paymentMethod: 'Apple Pay •••• 9012',
          transactionId: 'TXN-92104'
        }
      }
    ]
  },
  {
    id: 'tenant-nexcode',
    name: 'NexCode Advisory',
    slug: 'nexcode-advisory',
    subdomain: 'nexcode.slotly.io',
    tagline: 'Executive Tech Advisory, Cloud Architecture & Fractional CTO',
    category: 'Executive Advisory',
    badge: 'Enterprise Advisory',
    accentColor: '#f59e0b', // Amber
    accentGradient: 'from-amber-500 to-orange-600',
    logoIcon: 'layers',
    address: '500 Howard Street, Suite 1400, San Francisco, CA',
    phone: '+1 (415) 902-1840',
    email: 'partners@nexcode.io',
    stats: {
      bookingsMonth: 89,
      revenueMonth: 44500,
      occupancyRate: 88,
      rating: 4.99,
      retentionRate: 95
    },
    settings: {
      bookingNoticeHours: 24,
      cancelWindowHours: 48,
      allowInstantConfirmation: false,
      requireDeposit: true,
      depositPercent: 50,
      taxRatePercent: 9.0,
      processingFee: 5.00,
      allowTipping: false,
      currency: 'USD'
    },
    addons: [
      {
        id: 'addon-nex-1',
        name: 'Formal Systems Architecture Memo (PDF)',
        price: 150,
        durationMinutes: 0,
        description: 'Post-session blueprint detailing infrastructure recommendations.'
      }
    ],
    services: [
      {
        id: 'srv-nex-1',
        name: 'Distributed Cloud Architecture & Scalability Audit',
        category: 'Architecture',
        durationMinutes: 60,
        price: 500,
        description: 'Comprehensive review of multi-region infrastructure, latency bottlenecks, microservice topologies, and cost vectors.',
        popular: true,
        icon: 'layers'
      },
      {
        id: 'srv-nex-2',
        name: 'Fractional CTO Strategy & Org Design Session',
        category: 'Executive Advisory',
        durationMinutes: 90,
        price: 750,
        description: 'Engineering culture, technical debt remediation roadmap, compliance audit preparation, and hiring pipeline.',
        popular: true,
        icon: 'users'
      }
    ],
    staff: [
      {
        id: 'stf-nex-1',
        name: 'Alexandre Moreau',
        role: 'Chief Architect & Managing Partner',
        rating: 4.99,
        avatar: 'https://images.unsplash.com/photo-1500648767791-00dcc994a43e?w=150&auto=format&fit=crop&q=80',
        availableDays: ['Mon', 'Tue', 'Wed', 'Thu'],
        specialties: ['Distributed Systems', 'Cloud Cost Reduction']
      }
    ],
    bookings: [
      {
        id: 'bk-3001',
        clientName: 'Marcus Sterling',
        clientEmail: 'msterling@hyperflow.io',
        clientPhone: '+1 (650) 412-9901',
        serviceId: 'srv-nex-1',
        serviceName: 'Distributed Cloud Architecture & Scalability Audit',
        staffId: 'stf-nex-1',
        staffName: 'Alexandre Moreau',
        date: '2026-10-15',
        time: '11:00 AM',
        duration: 60,
        price: 500,
        status: 'confirmed',
        notes: 'Series B pre-scaling audit for multi-tenant database migration.',
        createdAt: '2026-10-12T11:00:00Z',
        pricing: {
          basePrice: 500,
          addons: [{ name: 'Formal Systems Architecture Memo (PDF)', price: 150 }],
          addonsTotal: 150,
          discount: 0,
          discountCode: null,
          subtotal: 650,
          tax: 58.50,
          processingFee: 5.00,
          tip: 0,
          grandTotal: 713.50,
          paymentOption: 'deposit',
          depositAmount: 356.75,
          balanceDue: 356.75,
          paymentMethod: 'Corporate Wire / Amex •••• 1004',
          transactionId: 'TXN-92399'
        }
      }
    ]
  }
]
