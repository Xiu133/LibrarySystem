const { createApp } = Vue;
createApp({
  data() {
    return { form: { name: '', description: '' }, message: '', success: false, loading: false };
  },
  methods: {
    async create() {
      if (!this.form.name.trim()) { this.message = '請輸入分類名稱'; this.success = false; return; }
      this.loading = true;
      const fd = new FormData();
      fd.append('name', this.form.name);
      if (this.form.description) fd.append('description', this.form.description);
      try {
        const res = await fetch('/Category/Create', { method: 'POST', body: fd });
        if (res.ok || res.redirected) {
          this.message = '✅ 新增成功';
          this.success = true;
          this.form = { name: '', description: '' };
          setTimeout(() => location.reload(), 800);
        }
      } catch { this.message = '新增失敗'; this.success = false; }
      this.loading = false;
    },
    async deleteCategory(id, name) {
      if (!confirm(`確定要刪除分類「${name}」？`)) return;
      const fd = new FormData();
      const res = await fetch(`/Category/Delete/${id}`, { method: 'POST', body: fd });
      const data = await res.json();
      if (data.success) location.reload();
      else alert(data.message || '刪除失敗');
    }
  }
}).mount('#app');
