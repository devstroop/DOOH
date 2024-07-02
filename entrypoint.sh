#!/bin/sh

# Set the certificate password from env
CERTIFICATE_PASSWORD=$CERTIFICATE_PASSWORD

# Generate a self-signed certificate
openssl req -x509 -newkey rsa:4096 -keyout /https/privateKey.key -out /https/certificate.crt -days 365 -subj "/C=US/ST=State/L=City/O=Organization/CN=localhost"

# Convert the certificate to PKCS#12 format
openssl pkcs12 -export -out /https/$CERTIFICATE_NAME.pfx -inkey /https/privateKey.key -in /https/certificate.crt -passout pass:$CERTIFICATE_PASSWORD

# Set the permissions
chmod 400 /https/$CERTIFICATE_NAME.pfx

# Start the application with HTTPS
exec dotnet DOOH.Server.dll