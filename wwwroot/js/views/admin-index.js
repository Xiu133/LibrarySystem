const ctx = document.getElementById('borrowChart');
if (ctx && typeof monthLabels !== 'undefined') {
  new Chart(ctx, {
    type: 'line',
    data: {
      labels: monthLabels,
      datasets: [{
        label: '借閱次數',
        data: monthCounts,
        borderColor: '#e94560',
        backgroundColor: 'rgba(233,69,96,0.08)',
        borderWidth: 2.5,
        fill: true,
        tension: 0.4,
        pointBackgroundColor: '#e94560',
        pointRadius: 5,
        pointHoverRadius: 7
      }]
    },
    options: {
      responsive: true,
      maintainAspectRatio: true,
      plugins: {
        legend: { display: false },
        tooltip: { backgroundColor: '#1a1a2e', titleFont: { size: 13 }, bodyFont: { size: 12 } }
      },
      scales: {
        x: { grid: { display: false }, ticks: { font: { size: 12 }, color: '#6c757d' } },
        y: { grid: { color: '#f0f0f0' }, ticks: { font: { size: 12 }, color: '#6c757d' }, beginAtZero: true }
      }
    }
  });
}
