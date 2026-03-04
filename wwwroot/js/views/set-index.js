const { createApp } = Vue;
createApp({
  data() { return { editing: {}, msg: '', ok: false }; },
  methods: {
    startEdit(id, val) { this.editing[id] = val; },
    async save(key, id) {
      const value = this.editing[id];
      if (value === undefined) return;
      const res = await fetch('/Set/Save', {
        method: 'POST',
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        body: `key=${encodeURIComponent(key)}&value=${encodeURIComponent(value)}`
      });
      const data = await res.json();
      this.ok = data.success;
      this.msg = data.success ? '✅ 已儲存' : (data.message || '儲存失敗');
      delete this.editing[id];
      if (data.success) setTimeout(() => location.reload(), 1000);
    },
    cancelEdit(id) { delete this.editing[id]; }
  }
}).mount('#app');
