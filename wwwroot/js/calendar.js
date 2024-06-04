document.addEventListener('DOMContentLoaded', () => {
    const monthLabel = document.getElementById('month-label');
    const prevButton = document.getElementById('prev');
    const nextButton = document.getElementById('next');
    const calendarBody = document.getElementById('calendar-body');
    let currentDate = new Date();

    const daysOfWeek = {
        "Sunday": "Dom",
        "Monday": "Seg",
        "Tuesday": "Ter",
        "Wednesday": "Qua",
        "Thursday": "Qui",
        "Friday": "Sex",
        "Saturday": "Sáb"
    };

    function renderCalendar() {
        const year = currentDate.getFullYear();
        const month = currentDate.getMonth();
        const firstDayOfMonth = new Date(year, month, 1);
        const lastDayOfMonth = new Date(year, month + 1, 0);
        const daysInMonth = lastDayOfMonth.getDate();
        const startDayOfWeek = firstDayOfMonth.getDay();

        if (monthLabel) {
            monthLabel.textContent = `Mês ${month + 1}/${year}`;
        }

        // Limpa o corpo do calendário
        calendarBody.innerHTML = '';

        // Renderizar as linhas da tabela de acordo com os dias do mês
        for (let i = 0; i < Model.WbsList.length; i++) {
            const row = document.createElement('tr');
            const wbsNameCell = document.createElement('td');
            wbsNameCell.textContent = Model.WbsList[i].Nome;
            row.appendChild(wbsNameCell);

            for (let day = 1; day <= daysInMonth; day++) {
                const cell = document.createElement('td');
                const date = new Date(year, month, day);
                const dayOfWeek = daysOfWeek[date.toLocaleDateString('en-US', { weekday: 'long' })];
                cell.innerHTML = `<div>${dayOfWeek}</div><div>${day}</div>`;
                row.appendChild(cell);
            }
            calendarBody.appendChild(row);
        }
    }

    prevButton.addEventListener('click', () => {
        currentDate.setMonth(currentDate.getMonth() - 1);
        renderCalendar();
    });

    nextButton.addEventListener('click', () => {
        currentDate.setMonth(currentDate.getMonth() + 1);
        renderCalendar();
    });

    renderCalendar(); // Chamar a função para renderizar o calendário inicialmente
});
