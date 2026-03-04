const { createApp } = Vue;
createApp({
  data() {
    return { userName: '', records: [], loading: false, searched: false, error: '', msg: '', ok: false };
  },
  methods: {
    async search() {
      const name = this.userName.trim();
      if (!name) return;
      this.loading = true;
      this.error = '';
      this.searched = false;
      const res = await fetch(`/ReturnBook/GetActiveRecords?userName=${encodeURIComponent(name)}`);
      const data = await res.json();
      this.loading = false;
      if (!data.success) { this.error = data.message; return; }
      this.records = data.records.map(r => ({ ...r, returning: false }));
      this.searched = true;
    },
    async returnBook(item) {
      item.returning = true;
      const fd = new FormData();
      fd.append('borrowRecordId', item.borrowRecordId);
      const res = await fetch('/ReturnBook/ReturnBook', { method: 'POST', body: fd });
      const data = await res.json();
      item.returning = false;
      if (data.success) {
        this.records = this.records.filter(r => r.borrowRecordId !== item.borrowRecordId);
        this.ok = true;
        this.msg = `✅ 《${item.title}》已歸還`;
        setTimeout(() => this.msg = '', 3000);
      } else {
        this.ok = false;
        this.msg = data.message || '歸還失敗';
      }
    }
  }
}).mount('#app');
