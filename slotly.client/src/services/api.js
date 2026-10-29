async function request(path, options = {}) {
  const response = await fetch(path, {
    ...options,
    headers: {
      Accept: 'application/json',
      ...(options.body ? { 'Content-Type': 'application/json' } : {}),
      ...options.headers
    }
  })

  if (!response.ok) {
    let message = `Request failed (${response.status})`
    try {
      const body = await response.text()
      if (body) {
        const parsed = JSON.parse(body)
        message = parsed.detail || parsed.title || parsed.message || body
      }
    } catch {
      // Keep the HTTP status message when the response body is not JSON.
    }
    throw new Error(message)
  }

  return response.status === 204 ? null : response.json()
}

function jsonBody(value) {
  return JSON.stringify(value)
}

export const slotlyApi = {
  getCategories: () => request('/api/Categories'),
  getStatuses: () => request('/api/Statuses'),
  getServices: (tenantId) => request(`/api/Services?tenantId=${encodeURIComponent(tenantId)}`),
  createService: (service) => request('/api/Services', {
    method: 'POST',
    body: jsonBody(service)
  }),
  deleteService: (id, tenantId) => request(
    `/api/Services/${encodeURIComponent(id)}?tenantId=${encodeURIComponent(tenantId)}`, {
    method: 'DELETE'
  }),
  getAddons: (tenantId) => request(`/api/ServiceAddons?tenantId=${encodeURIComponent(tenantId)}`),
  createAddon: (addon) => request('/api/ServiceAddons', {
    method: 'POST',
    body: jsonBody(addon)
  }),
  deleteAddon: (id) => request(`/api/ServiceAddons/${encodeURIComponent(id)}`, {
    method: 'DELETE'
  }),
  getBookings: (tenantId) => request(`/api/Bookings?tenantId=${encodeURIComponent(tenantId)}`),
  createBooking: (booking) => request('/api/Bookings', {
    method: 'POST',
    body: jsonBody(booking)
  }),
  updateBooking: (booking) => request(`/api/Bookings/${encodeURIComponent(booking.id)}`, {
    method: 'PUT',
    body: jsonBody(booking)
  })
}
