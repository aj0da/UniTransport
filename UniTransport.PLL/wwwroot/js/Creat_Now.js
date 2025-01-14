// Wait for the DOM to be fully loaded
document.addEventListener('DOMContentLoaded', function() {
    const form = document.querySelector('.form-section');
    const submitBtn = document.getElementById('submit-btn');

    // Add click event listener to the submit button
    submitBtn.addEventListener('click', function(event) {
        event.preventDefault();
        
        // Get all form values
        const from = document.getElementById('from').value;
        const to = document.getElementById('to').value;
        const date = document.getElementById('date').value;
        const time = document.getElementById('time').value;

        // Basic validation
        if (!from || !to || !date || !time) {
            alert('Please fill in all required fields');
            return;
        }

        // Validate date (must be future date)
        const selectedDate = new Date(date);
        const today = new Date();
        today.setHours(0, 0, 0, 0);

        if (selectedDate < today) {
            alert('Please select a future date');
            return;
        }

        // Show confirmation dialog
        const confirmMessage = `Please confirm your trip details:\n\nFrom: ${from}\nTo: ${to}\nDate: ${date}\nTime: ${time}`;
        
        if (confirm(confirmMessage)) {
            // Create trip object
            //const tripData = {
            //    from: from,
            //    to: to,
            //    date: date,
            //    time: time,
            //    createdAt: new Date().toISOString()
            //};

            // Save to localStorage
            //saveTrip(tripData);

            // Show success message
            alert('Trip created successfully!');

            // Reset form
            //resetForm();
        }
    });
});

// Function to save trip to localStorage
function saveTrip(tripData) {
    // Get existing trips or initialize empty array
    let trips = JSON.parse(localStorage.getItem('trips') || '[]');
    
    // Add new trip
    trips.push(tripData);
    
    // Save back to localStorage
    localStorage.setItem('trips', JSON.stringify(trips));
}

// Function to reset form
function resetForm() {
    document.getElementById('from').value = '';
    document.getElementById('to').value = '';
    document.getElementById('date').value = '';
    document.getElementById('time').value = '';
}