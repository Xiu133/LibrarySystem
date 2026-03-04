const { createApp } = Vue;
createApp({
  data() {
    return {
      query: '', books: initialBooks, loading: false, timer: null,
      drawerOpen: false, drawerLoading: false, drawerBook: null
    };
  },
  methods: {
    search() {
      clearTimeout(this.timer);
      if (!this.query.trim()) { this.books = initialBooks; return; }
      this.timer = setTimeout(async () => {
        this.loading = true;
        const res = await fetch(`/Book/Search?query=${encodeURIComponent(this.query)}`);
        this.books = await res.json();
        this.loading = false;
      }, 300);
    },
    reset() {
      this.query = '';
      this.books = initialBooks;
    },
    async openDrawer(id) {
      this.drawerOpen = true;
      this.drawerLoading = true;
      this.drawerBook = null;
      document.body.style.overflow = 'hidden';
      const res = await fetch(`/Book/GetDetail?id=${id}`);
      this.drawerBook = await res.json();
      this.drawerLoading = false;
    },
    closeDrawer() {
      this.drawerOpen = false;
      document.body.style.overflow = '';
    }
  }
}).mount('#app');
