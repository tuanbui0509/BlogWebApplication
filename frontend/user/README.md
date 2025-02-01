This is a [Next.js](https://nextjs.org) project bootstrapped with [`create-next-app`](https://nextjs.org/docs/app/api-reference/cli/create-next-app).

## Getting Started

First, run the development server:

```bash
nextjs-blog/
├── public/                  # Static assets (images, fonts, etc.)
│   ├── images/
│   │   └── logo.png
│   └── favicon.ico
│
├── src/                     # Main source code
│   ├── components/          # Reusable UI components
│   │   ├── Header.js
│   │   ├── Footer.js
│   │   └── Layout.js
│   │
│   ├── hooks/               # Custom React hooks
│   │   └── useTheme.js
│   │
│   ├── lib/                 # Utility functions and libraries
│   │   └── posts.js         # Markdown parsing logic
│   │
│   ├── pages/               # Next.js pages
│   │   ├── index.js         # Homepage
│   │   ├── posts/           # Blog posts
│   │   │   └── [id].js      # Dynamic post page
│   │   ├── _app.js          # Custom App component
│   │   └── _document.js     # Custom Document component
│   │
│   ├── services/            # API services
│   │   └── api.js           # Centralized API calls
│   │
│   ├── styles/              # Global and component styles
│   │   ├── globals.css      # Global Tailwind CSS
│   │   └── Home.module.css  # Component-specific styles
│   │
│   └── tests/               # Unit tests
│       ├── components/      # Tests for components
│       │   └── Header.test.js
│       ├── pages/           # Tests for pages
│       │   └── Home.test.js
│       └── lib/             # Tests for utility functions
│           └── posts.test.js
│
├── posts/                   # Markdown blog posts
│   ├── post1.md
│   └── post2.md
│
├── .env.local               # Environment variables
├── jest.config.js           # Jest configuration
├── next.config.js           # Next.js configuration
├── tailwind.config.js       # Tailwind CSS configuration
├── package.json             # Project dependencies
└── README.md                # Project documentation
```