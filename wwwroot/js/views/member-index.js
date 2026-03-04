const { createApp } = Vue;
createApp({
  data() {
    return {
      search: '', msg: '', ok: false,
      modal: { open: false, loading: false, member: null }
    };
  },
  methods: {
    async openModal(userName) {
      this.modal.open = true;
      this.modal.loading = true;
      this.modal.member = null;
      const res = await fetch(`/MemberManage/GetMemberDetail?userName=${encodeURIComponent(userName)}`);
      this.modal.member = await res.json();
      this.modal.loading = false;
    },
    closeModal() {
      this.modal.open = false;
    },
    async suspend(userName) {
      if (!confirm(`確定停權會員「${userName}」？`)) return;
      const res = await fetch('/MemberManage/Suspend', {
        method: 'POST',
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        body: `userName=${encodeURIComponent(userName)}`
      });
      const data = await res.json();
      this.ok = data.success;
      this.msg = data.success ? '✅ 已停權' : (data.message || '操作失敗');
      if (data.success) {
        this.modal.open = false;
        setTimeout(() => location.reload(), 1000);
      }
    },
    async activate(userName) {
      if (!confirm(`確定啟用會員「${userName}」？`)) return;
      const res = await fetch('/MemberManage/Activate', {
        method: 'POST',
        headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
        body: `userName=${encodeURIComponent(userName)}`
      });
      const data = await res.json();
      this.ok = data.success;
      this.msg = data.success ? '✅ 已啟用' : (data.message || '操作失敗');
      if (data.success) {
        this.modal.open = false;
        setTimeout(() => location.reload(), 1000);
      }
    }
  }
}).mount('#app');
