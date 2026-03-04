const { createApp } = Vue;
createApp({
  data() {
    return { query: '', books: [], selected: [], loading: false, resultMsg: '', resultSuccess: false, timer: null };
  },
  mounted() { this.loadAll(); },
  methods: {
    async loadAll() {
      this.loading = true;
      try {
        const res = await fetch('/Book/Search');
        this.books = await res.json();
      } catch { this.books = []; }
      this.loading = false;
    },
    search() {
      clearTimeout(this.timer);
      if (!this.query.trim()) { this.loadAll(); return; }
      this.timer = setTimeout(async () => {
        this.loading = true;
        const res = await fetch(`/Book/Search?query=${encodeURIComponent(this.query)}`);
        this.books = await res.json();
        this.loading = false;
      }, 300);
    },
    isSelected(id) { return this.selected.some(b => b.id === id); },
    toggleSelect(book) {
      if (this.isSelected(book.id)) this.selected = this.selected.filter(b => b.id !== book.id);
      else this.selected.push(book);
    },
    remove(id) { this.selected = this.selected.filter(b => b.id !== id); },
    async confirm() {
      if (!this.selected.length) return;
      const res = await fetch('/Reserve/ConfirmReserve', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ SelectBooks: this.selected.map(b => b.id) })
      });
      const data = await res.json();
      this.resultSuccess = data.success;
      this.resultMsg = data.message;
      if (data.success) { this.selected = []; this.loadAll(); }
    }
  }
}).mount('#app');
