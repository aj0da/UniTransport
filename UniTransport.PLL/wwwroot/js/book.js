// Initialize selectedTrips array if not already defined
if (typeof window.selectedTrips === 'undefined') {
    window.selectedTrips = [];
}

// Initialize when document loads
document.addEventListener('DOMContentLoaded', function() {
    const bookingDate = document.getElementById('bookingDate');
    const dateAlert = document.getElementById('dateAlert');
    const buttons = document.querySelectorAll('.add');
    
    // Set minimum date to today
    const today = new Date().toISOString().split('T')[0];
    bookingDate.min = today;
    
    // Initially disable buttons
    buttons.forEach(btn => {
        btn.disabled = true;
        btn.style.opacity = '0.5';
        btn.style.cursor = 'not-allowed';
    });
    
    // Handle date selection
    bookingDate.addEventListener('change', function() {
        if (this.value) {
            dateAlert.textContent = 'Date selected - You can now book your trip';
            dateAlert.style.background = 'rgba(36, 161, 123, 0.2)';
            dateAlert.style.color = '#24a17b';
            
            buttons.forEach(btn => {
                btn.disabled = false;
                btn.style.opacity = '1';
                btn.style.cursor = 'pointer';
            });
        } else {
            dateAlert.textContent = 'Please select a date to see available trips';
            dateAlert.style.background = 'rgba(36, 161, 123, 0.1)';
            dateAlert.style.color = '#666';
            
            buttons.forEach(btn => {
                btn.disabled = true;
                btn.style.opacity = '0.5';
                btn.style.cursor = 'not-allowed';
            });
        }
    });
});

function toggleTrip(button) {
    const row = button.closest('tr');
    const tripData = {
        carNumber: row.cells[0].textContent,
        destination: row.cells[1].textContent,
        seats: row.cells[2].textContent,
        time: row.cells[3].textContent,
        price: parseFloat(row.cells[4].textContent) || 0
    };
    const seatsCell = row.querySelector('.seats'); // Assuming the 'Seats' column has a class `seats`
    const hiddenInput = row.querySelector('.is-booked'); // Assuming hidden input for `IsDeleted`

    let availableSeats = parseInt(seatsCell.textContent, 10);

    if (button.classList.contains('selected')) {
        // Remove trip from selection
        button.classList.remove('selected');
        button.textContent = 'Add';
        window.selectedTrips = window.selectedTrips.filter(trip => trip.carNumber !== tripData.carNumber || trip.time !== tripData.time);
        availableSeats++;
        hiddenInput.value = 'false'; // Mark as not booked
    } else {
        // Add trip to selection
        button.classList.add('selected');
        button.textContent = 'Selected';
        availableSeats--;
        hiddenInput.value = 'true'; // Mark as booked
        window.selectedTrips.push(tripData);
    }

    // Update seats display
    seatsCell.textContent = availableSeats;

    updateSelectedTripsUI();
}

function updateSelectedTripsUI() {
    const selectedCount = document.getElementById('selected-count');
    const footerSelectedCount = document.getElementById('footer-selected-count');
    const footerTotalPrice = document.getElementById('footer-total-price');
    const selectedTripsList = document.getElementById('selected-trips-list');
    const bookAllBtn = document.getElementById('book-all-btn');

    // Update counts
    selectedCount.textContent = window.selectedTrips.length;
    footerSelectedCount.textContent = window.selectedTrips.length;

    // Calculate and update total price
    const totalPrice = window.selectedTrips.reduce((sum, trip) => sum + trip.price, 0);
    footerTotalPrice.textContent = totalPrice.toFixed(2);

    // Update selected trips list
    selectedTripsList.innerHTML = '';
    window.selectedTrips.forEach(trip => {
        const tripElement = document.createElement('div');
        tripElement.className = 'selected-trip-item';
        tripElement.innerHTML = `
            <span>${trip.destination} - ${trip.time} (${trip.carNumber})</span>
            <span>${trip.price.toFixed(2)}</span>
        `;
        selectedTripsList.appendChild(tripElement);
    });

    // Enable/disable book all button based on selection
    bookAllBtn.disabled = window.selectedTrips.length === 0;
}

// Add event listener for the book all button
document.getElementById('book-all-btn').addEventListener('click', function(event) {
    if (window.selectedTrips.length > 0) {
        event.preventDefault();
        
        // Show confirmation with total price
        const totalPrice = window.selectedTrips.reduce((sum, trip) => sum + trip.price, 0);
        alert(`Successfully booked ${window.selectedTrips.length} trips!\nTotal Price: ${totalPrice.toFixed(2)}`);
        
        // Reset selection after successful booking
        window.selectedTrips = [];
        document.querySelectorAll('.add.selected').forEach(button => {
            button.classList.remove('selected');
            button.textContent = 'Add';
        });
        updateSelectedTripsUI();
        
        // Redirect to profile page after successful booking
        document.getElementById('bookForm').submit();
    }
});

function change(button) {
    const date = document.getElementById('bookingDate').value;
    if (!date) {
        document.getElementById('dateAlert').textContent = 'Please select a date first!';
        return;
    }
    
    const row = button.closest('tr');
    const booking = {
        carNumber: row.cells[0].textContent,
        destination: row.cells[1].textContent,
        date: date,
        timestamp: new Date().toISOString()
    };
    
    // Save booking
    const bookings = JSON.parse(localStorage.getItem('bookings') || '[]');
    bookings.push(booking);
    localStorage.setItem('bookings', JSON.stringify(bookings));
    
    // Update button
    button.textContent = 'Booked';
    button.style.backgroundColor = '#4CAF50';
    button.disabled = true;
    button.style.cursor = 'default';
    
    // Show success message and redirect
    alert('Booking successful!');
    setTimeout(() => {
        window.location.href = 'profile.html';
    }, 1000);
}
