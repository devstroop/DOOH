#!/bin/sh

# Set the certificate password from env
CERTIFICATE_PASSWORD=$CERTIFICATE_PASSWORD

# Generate a self-signed certificate
openssl req -x509 -newkey rsa:4096 -keyout /app/privateKey.key -out /app/certificate.crt -days 365 -subj "/C=US/ST=State/L=City/O=Organization/CN=localhost"

# Convert the certificate to PKCS#12 format
openssl pkcs12 -export -out /app/aspnetapp.pfx -inkey /app/privateKey.key -in /app/certificate.crt -passout pass:$CERTIFICATE_PASSWORD

# Assuming your generated file is aspnetapp.pfx
mv /app/aspnetapp.pfx $ASPNETCORE_Kestrel__Certificates__Default__Path

# Set the permissions
chmod 400 $ASPNETCORE_Kestrel__Certificates__Default__Path

# Start the application with HTTPS
exec dotnet DOOH.Server.dll