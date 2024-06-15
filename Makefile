#!/usr/bin/make

build:
	docker build -t beatmaps-service:latest -t registry.digitalocean.com/akatsuki/beatmaps-service:latest .

run:
	docker-compose up beatmaps-service

run-bg:
	docker-compose up -d beatmaps-service

stop:
	docker-compose down

logs:
	docker-compose logs -f