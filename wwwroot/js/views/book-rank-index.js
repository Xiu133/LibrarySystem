// Animate rank bars on load
document.addEventListener('DOMContentLoaded', () => {
  const maxCount = parseInt(document.querySelector('.rank-bar')?.dataset.max || '1');
  document.querySelectorAll('.rank-bar').forEach(bar => {
    const count = parseInt(bar.dataset.count || '0');
    const pct = maxCount > 0 ? Math.max(4, (count / maxCount) * 100) : 4;
    setTimeout(() => { bar.style.width = pct + '%'; }, 100);
  });
});
