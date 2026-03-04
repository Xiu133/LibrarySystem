async function loadCharts() {
  const incomeRes = await fetch('/FinanceTotal/GetIncomeData');
  const incomeData = await incomeRes.json();

  const pieCtx = document.getElementById('incomePieChart');
  if (pieCtx && incomeData.length) {
    new Chart(pieCtx, {
      type: 'doughnut',
      data: {
        labels: incomeData.map(d => d.reason),
        datasets: [{
          data: incomeData.map(d => d.total),
          backgroundColor: ['#e94560', '#1a1a2e', '#10b981', '#f59e0b', '#3b82f6', '#8b5cf6'],
          borderWidth: 2,
          borderColor: '#fff'
        }]
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { position: 'bottom', labels: { font: { size: 13 }, padding: 16 } },
          tooltip: { backgroundColor: '#1a1a2e' }
        }
      }
    });
  }

  try {
    const expRes = await fetch('/FinanceTotal/GetExpenseData');
    const expData = await expRes.json();
    const barCtx = document.getElementById('expenseBarChart');
    if (barCtx && expData.length) {
      new Chart(barCtx, {
        type: 'bar',
        data: {
          labels: expData.map(d => d.month),
          datasets: [{
            label: '月支出',
            data: expData.map(d => d.total),
            backgroundColor: 'rgba(233,69,96,0.15)',
            borderColor: '#e94560',
            borderWidth: 2,
            borderRadius: 6
          }]
        },
        options: {
          responsive: true,
          maintainAspectRatio: false,
          plugins: { legend: { display: false }, tooltip: { backgroundColor: '#1a1a2e' } },
          scales: {
            x: { grid: { display: false }, ticks: { color: '#6c757d' } },
            y: { grid: { color: '#f0f0f0' }, ticks: { color: '#6c757d' }, beginAtZero: true }
          }
        }
      });
    }
  } catch {}
}

loadCharts();
