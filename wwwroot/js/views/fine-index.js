const { createApp } = Vue;
createApp({
  data() {
    return { filter: 'all', message: '', success: false };
  },
  methods: {
    shouldShow(isPaidStr) {
      const isPaid = isPaidStr === 'True' || isPaidStr === 'true';
      if (this.filter === 'unpaid') return !isPaid;
      if (this.filter === 'paid') return isPaid;
      return true;
    },
    async markPaid(id) {
      const res = await fetch(`/Fine/MarkPaid/${id}`, {
        method: 'POST',
        headers: { 'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]')?.value || '' }
      });
      const data = await res.json();
      this.success = data.success;
      this.message = data.success ? '✅ 已標記為繳清' : (data.message || '操作失敗');
      if (data.success) setTimeout(() => location.reload(), 1200);
    }
  }
}).mount('#app');
