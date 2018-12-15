echo "$ENCRYPTED_DOCKER_PASSWORD" | docker login -u "$ENCRYPTED_DOCKER_USERNAME" --password-stdin
cd HealthDataRepository
docker build -t sem56402018/health-data-repository:$1 -t sem56402018/health-data-repository:$TRAVIS_COMMIT .
docker push sem56402018/health-data-repository:$TRAVIS_COMMIT
docker push sem56402018/health-data-repository:$1
