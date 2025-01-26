# CI_REGISTRY Explanation

CI_REGISTRY is a predefined GitLab CI/CD variable that contains the URL of the GitLab Container Registry. This variable is automatically set by GitLab in every CI/CD pipeline.

## Details
- CI_REGISTRY is automatically configured by GitLab
- It contains the URL of the project's container registry (e.g., registry.gitlab.com)
- It is commonly used in CI/CD pipelines for container image operations
- No manual configuration is required as GitLab sets this automatically

## Related Variables
CI_REGISTRY is often used with other registry-related variables:
- CI_REGISTRY_USER: Registry user for authentication
- CI_REGISTRY_PASSWORD: Registry password for authentication
- CI_REGISTRY_IMAGE: Full image name for the project's registry

## Usage Example
The CI_REGISTRY variable is typically used in Docker operations within CI/CD pipelines, such as:
```yaml
docker login $CI_REGISTRY -u $CI_REGISTRY_USER -p $CI_REGISTRY_PASSWORD
docker build -t $CI_REGISTRY_IMAGE .
docker push $CI_REGISTRY_IMAGE
```