const { createApp } = Vue;
createApp({
  data() { return { search: '' }; },
  methods: {
    async deleteExpense(id) {
      if (!confirm('確定要刪除此費用記錄？')) return;
      const fd = new FormData();
      const res = await fetch(`/Expense/Delete/${id}`, { method: 'POST', body: fd });
      const data = await res.json();
      if (data.success) location.reload();
      else alert('刪除失敗');
    }
  }
}).mount('#app');
