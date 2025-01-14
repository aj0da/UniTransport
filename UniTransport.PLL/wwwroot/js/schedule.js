// Schedule Management JavaScript

let editingTripId = null;

function showAddTripModal() {
    editingTripId = null;
    $('#tripModalTitle').text('Add Trip');
    $('#tripForm')[0].reset();
    loadVehicles();
    $('#tripModal').modal('show');
}

function editTrip(tripId) {
    editingTripId = tripId;
    $('#tripModalTitle').text('Edit Trip');
    
    // Load trip details
    $.get(`/Admin/GetTrip/${tripId}`, function(response) {
        if (response.success) {
            const trip = response.trip;
            $('#vehicleId').val(trip.vehicleId);
            $('#price').val(trip.price);
            $('#departureLocation').val(trip.departureLocation);
            $('#arrivalLocation').val(trip.arrivalLocation);
            $('#departureTime').val(trip.departureTime.slice(0, 16)); // Format for datetime-local input
            $('#arrivalTime').val(trip.arrivalTime.slice(0, 16));
            
            loadVehicles(trip.vehicleId);
            $('#tripModal').modal('show');
        } else {
            toastr.error(response.message || 'Error loading trip details');
        }
    });
}

function loadVehicles(selectedVehicleId = null) {
    $.get('/Admin/GetActiveVehicles', function(response) {
        if (response.success) {
            const select = $('#vehicleId');
            select.empty();
            select.append('<option value="">Select a vehicle</option>');
            
            response.vehicles.forEach(vehicle => {
                const option = $('<option>')
                    .val(vehicle.vehicleId)
                    .text(`${vehicle.licensePlate} (${vehicle.vehicleType} - Capacity: ${vehicle.capacity})`);
                    
                if (vehicle.vehicleId === selectedVehicleId) {
                    option.prop('selected', true);
                }
                
                select.append(option);
            });
        } else {
            toastr.error('Error loading vehicles');
        }
    });
}

function saveTrip() {
    const tripData = {
        tripId: editingTripId,
        vehicleId: $('#vehicleId').val(),
        price: parseFloat($('#price').val()),
        departureLocation: $('#departureLocation').val(),
        arrivalLocation: $('#arrivalLocation').val(),
        departureTime: $('#departureTime').val(),
        arrivalTime: $('#arrivalTime').val()
    };

    // Validation
    if (!validateTripData(tripData)) {
        return;
    }

    const url = editingTripId ? '/Admin/UpdateTrip' : '/Admin/AddTrip';
    
    $.ajax({
        url: url,
        type: 'POST',
        data: tripData,
        success: function(response) {
            if (response.success) {
                $('#tripModal').modal('hide');
                toastr.success(response.message);
                refreshScheduleTable();
            } else {
                toastr.error(response.message);
            }
        },
        error: function() {
            toastr.error('An error occurred while saving the trip');
        }
    });
}

function validateTripData(tripData) {
    if (!tripData.vehicleId) {
        toastr.error('Please select a vehicle');
        return false;
    }
    
    if (tripData.price <= 0) {
        toastr.error('Price must be greater than 0');
        return false;
    }
    
    if (!tripData.departureLocation || !tripData.arrivalLocation) {
        toastr.error('Please enter both departure and arrival locations');
        return false;
    }
    
    const departureTime = new Date(tripData.departureTime);
    const arrivalTime = new Date(tripData.arrivalTime);
    
    if (arrivalTime <= departureTime) {
        toastr.error('Arrival time must be after departure time');
        return false;
    }
    
    return true;
}

function deleteTrip(tripId) {
    if (confirm('Are you sure you want to delete this trip? This action cannot be undone.')) {
        $.ajax({
            url: '/Admin/DeleteTrip',
            type: 'POST',
            data: { tripId: tripId },
            success: function(response) {
                if (response.success) {
                    toastr.success(response.message);
                    refreshScheduleTable();
                } else {
                    toastr.error(response.message);
                }
            },
            error: function() {
                toastr.error('An error occurred while deleting the trip');
            }
        });
    }
}

function refreshScheduleTable() {
    $.get('/Admin/Schedules', function(html) {
        $('#scheduleTableContainer').html(html);
    });
}

// Event Handlers
$(document).ready(function() {
    // Handle form submission
    $('#tripForm').on('submit', function(e) {
        e.preventDefault();
        saveTrip();
    });
    
    // Validate arrival time when departure time changes
    $('#departureTime').on('change', function() {
        const departureTime = new Date($(this).val());
        const arrivalTime = new Date($('#arrivalTime').val());
        
        if (arrivalTime <= departureTime) {
            $('#arrivalTime').val('');
            toastr.warning('Arrival time must be after departure time');
        }
    });
    
    // Initialize tooltips
    $('[data-bs-toggle="tooltip"]').tooltip();
});
