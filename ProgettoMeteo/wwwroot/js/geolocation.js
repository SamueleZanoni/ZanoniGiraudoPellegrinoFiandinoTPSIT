document.addEventListener('DOMContentLoaded', function () {
    const locationForm = document.getElementById('locationForm');
    const latInput = document.getElementById('lat');
    const lonInput = document.getElementById('lon');
    const currentLocationBtn = document.getElementById('currentLocationBtn');
    const statusMessageEl = document.getElementById('statusMessage');

    // Detailed logging function
    function logAndDisplayError(message) {
        console.error(message);
        statusMessageEl.textContent = message;
    }

    // Check browser support and permissions
    function checkGeolocationSupport() {
        // Check if geolocation is supported
        if (!("geolocation" in navigator)) {
            logAndDisplayError("Geolocation is not supported by this browser.");
            return false;
        }

        // Check if permissions are already granted
        if (navigator.permissions) {
            navigator.permissions.query({ name: 'geolocation' }).then(function (result) {
                if (result.state === 'granted') {
                    console.log('Geolocation permission already granted');
                } else if (result.state === 'prompt') {
                    console.log('Geolocation permission will be prompted');
                } else {
                    logAndDisplayError('Geolocation permission denied');
                    return false;
                }
            });
        }

        return true;
    }

    currentLocationBtn.addEventListener('click', function (e) {
        e.preventDefault();

        // Reset previous status
        statusMessageEl.textContent = '';

        // Disable button during geolocation
        currentLocationBtn.classList.add('disabled');
        currentLocationBtn.querySelector('.span').textContent = 'Locating...';

        // Perform initial support check
        if (!checkGeolocationSupport()) {
            currentLocationBtn.classList.remove('disabled');
            currentLocationBtn.querySelector('.span').textContent = 'Current Location';
            return;
        }

        // Attempt to get location
        navigator.geolocation.getCurrentPosition(
            function (position) {
                // Success callback
                const lat = position.coords.latitude;
                const lon = position.coords.longitude;

                console.log('Location obtained:', {
                    latitude: lat,
                    longitude: lon,
                    accuracy: position.coords.accuracy
                });

                // Convert coordinates to comma-separated format
                const latComma = lat.toString().replace('.', ',');
                const lonComma = lon.toString().replace('.', ',');

                // Set hidden input values
                latInput.value = latComma;
                lonInput.value = lonComma;

                // Submit form
                locationForm.submit();
            },
            function (error) {
                // Error callback
                let errorMessage = "Unknown error";
                switch (error.code) {
                    case error.PERMISSION_DENIED:
                        errorMessage = "Location permission denied. Please enable location in your browser settings.";
                        break;
                    case error.POSITION_UNAVAILABLE:
                        errorMessage = "Location information is unavailable. Check your network connection.";
                        break;
                    case error.TIMEOUT:
                        errorMessage = "Location request timed out. Please try again.";
                        break;
                }

                // Log detailed error
                logAndDisplayError(errorMessage);
                console.error('Geolocation error:', error);

                // Reset button
                currentLocationBtn.classList.remove('disabled');
                currentLocationBtn.querySelector('.span').textContent = 'Current Location';
            },
            {
                enableHighAccuracy: true,
                timeout: 10000,
                maximumAge: 0
            }
        );
    });
});
