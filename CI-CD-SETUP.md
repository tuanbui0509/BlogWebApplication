# CI/CD Setup Documentation

This document describes the CI/CD pipeline setup for the WebBlogApplication.

## Pipeline Structure

The CI/CD pipeline is configured using GitLab CI/CD and consists of the following stages:

1. Build
2. Test
3. Security
4. Docker
5. Deploy

## Service Pipelines

### Backend Service
- Builds and tests Python application
- Creates Docker image
- Pushes to GitLab Container Registry

### Frontend Admin Service
- Builds and tests Node.js application
- Creates Docker image
- Pushes to GitLab Container Registry

### Frontend User Service
- Builds and tests Node.js application
- Creates Docker image
- Pushes to GitLab Container Registry

## Environments

### Staging
- Automatically deployed when changes are merged to main branch
- Uses latest Docker images tagged with commit SHA

### Production
- Manual deployment triggered with Git tags
- Uses versioned Docker images tagged with Git tag

## Required GitLab Variables

The following variables need to be set in GitLab CI/CD settings:

- `CI_REGISTRY`: GitLab Container Registry URL (auto-set by GitLab)
- `CI_REGISTRY_USER`: Registry user (auto-set by GitLab)
- `CI_REGISTRY_PASSWORD`: Registry password (auto-set by GitLab)
- `CI_REGISTRY_IMAGE`: Full image name (auto-set by GitLab)

## Deployment Process

1. Staging deployment:
   - Automatic when merging to main branch
   - Updates services with latest images

2. Production deployment:
   - Manual trigger required
   - Requires Git tag
   - Uses tagged versions of images