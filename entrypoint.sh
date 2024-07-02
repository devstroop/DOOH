#!/bin/bash
# entrypoint.sh
# Exit immediately if a command exits with a non-zero status
set -e

# Function to handle errors
error_handler() {
    echo "Error occurred in script at line: $1."
    exit 1
}

# Trap errors and pass the line number to the error handler
trap 'error_handler $LINENO' ERR

# Set the certificate password from env
CERTIFICATE_PASSWORD=${CERTIFICATE_PASSWORD:-defaultpassword}
CERTIFICATE_PATH=${ASPNETCORE_Kestrel__Certificates__Default__Path:-/https/aspnetapp.pfx}

# Ensure the /https directory exists and is writable
mkdir -p /https
chmod 755 /https

# Check if the certificate already exists
if [ ! -f "$CERTIFICATE_PATH" ]; then
    echo "Certificate not found. Generating a new one..."

    # Generate a self-signed certificate
    openssl req -x509 -newkey rsa:4096 -nodes -keyout /https/privateKey.key -out /https/certificate.crt -days 365 -subj "/C=US/ST=State/L=City/O=Organization/CN=localhost"

    # Convert the certificate to PKCS#12 format
    openssl pkcs12 -export -out /https/aspnetapp.pfx -inkey /https/privateKey.key -in /https/certificate.crt -passout pass:$CERTIFICATE_PASSWORD

    # Move the generated file to the specified path
    mkdir -p "$(dirname "$CERTIFICATE_PATH")"
    mv /https/aspnetapp.pfx "$CERTIFICATE_PATH"

    # Set the permissions
    chmod 400 "$CERTIFICATE_PATH"

    echo "Certificate generated and moved to $CERTIFICATE_PATH"
else
    echo "Using existing certificate at $CERTIFICATE_PATH"
fi

exec dotnet DOOH.Server.dll
