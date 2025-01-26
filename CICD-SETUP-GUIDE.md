# CI/CD Setup Guide for WebBlogApplication

This guide will walk you through setting up the CI/CD pipeline for the WebBlogApplication.

## Prerequisites

1. GitLab account and repository
2. Docker installed on your local machine
3. Access to GitLab Container Registry

## Step 1: Configure GitLab Variables

In your GitLab project:
1. Go to Settings > CI/CD > Variables
2. The following variables are automatically set by GitLab:
   - `CI_REGISTRY`
   - `CI_REGISTRY_USER`
   - `CI_REGISTRY_PASSWORD`
   - `CI_REGISTRY_IMAGE`

## Step 2: Pipeline Structure

The pipeline is organized into the following stages:
1. Build: Compiles and builds the applications
2. Test: Runs unit and integration tests
3. Docker: Creates and pushes Docker images
4. Deploy: Deploys to staging/production environments

## Step 3: Service Setup

### Backend Service
1. Place your Python application code in the `backend/` directory
2. Create a `Dockerfile` in the backend directory
3. Configure tests in `pytest`

### Frontend Admin Service
1. Place your admin frontend code in `frontend/admin/`
2. Create a `Dockerfile` in the admin directory
3. Configure tests using Jest

### Frontend User Service
1. Place your user frontend code in `frontend/user/`
2. Create a `Dockerfile` in the user directory
3. Configure tests using Jest

## Step 4: Deployment Configuration

### Staging Environment
- Automatic deployment on merges to main branch
- Uses Docker images tagged with commit SHA
- Access the staging environment at your configured staging URL

### Production Environment
- Manual deployment using Git tags
- Uses versioned Docker images
- Triggered through GitLab interface

## Step 5: Running the Pipeline

1. Commit and push your changes to GitLab
2. Pipeline will automatically start for:
   - Push to any branch (build and test stages)
   - Merge to main (includes staging deployment)
   - Creating a Git tag (enables production deployment)

3. Monitor pipeline progress in GitLab CI/CD > Pipelines

## Step 6: Deployment Process

### For Staging:
1. Merge changes to main branch
2. Pipeline automatically deploys to staging
3. Images are tagged with commit SHA

### For Production:
1. Create a Git tag (e.g., v1.0.0)
2. Go to GitLab CI/CD > Pipelines
3. Find the pipeline for your tag
4. Manually trigger production deployment

## Troubleshooting

1. Check pipeline logs in GitLab CI/CD > Pipelines
2. Ensure all required variables are set
3. Verify Docker registry access
4. Check your Dockerfile configurations

## Security Notes

- Never commit sensitive information
- Use GitLab variables for secrets
- Regularly update dependencies
- Follow security best practices in Dockerfiles