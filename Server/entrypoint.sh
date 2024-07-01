#!/bin/sh

# Set the certificate password from env
CERTIFICATE_PASSWORD=$CERTIFICATE_PASSWORD

# Generate a self-signed certificate
openssl req -x509 -newkey rsa:4096 -keyout /app/privateKey.key -out /app/certificate.crt -days 365 -subj "/C=US/ST=State/L=City/O=Organization/CN=localhost"

# Convert the certificate to PKCS#12 format
openssl pkcs12 -export -out /app/aspnetapp.pfx -inkey /app/privateKey.key -in /app/certificate.crt -passout pass:$CERTIFICATE_PASSWORD

# Set the permissions
chmod 400 /app/aspnetapp.pfx

# Start the application with HTTPS
dotnet DOOH.Server.dll