const { createApp } = Vue;
createApp({
  data() {
    return { userName: '', confirmed: false, loading: false, error: '' };
  },
  methods: {
    async confirmUser() {
      const name = this.userName.trim();
      if (!name) return;
      this.loading = true;
      this.error = '';
      this.confirmed = false;
      const res = await fetch(`/Income/ConfirmUser?userName=${encodeURIComponent(name)}`);
      const data = await res.json();
      this.loading = false;
      if (data.success) {
        this.confirmed = true;
      } else {
        this.error = data.message || '查詢失敗';
      }
    }
  }
}).mount('#app');
