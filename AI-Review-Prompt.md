# 🤖 AI Review Prompt - Notely Project Structure Analysis

## 📋 Prompt Template

```
I'm developing a .NET 10 Clean Architecture project called "Notely" (a note-taking application) using Blazor Server/WebAssembly hybrid, CQRS pattern, and Entity Framework Core. I need a comprehensive architectural review and optimization recommendations.

## 📁 Current Project Structure Analysis
[Attach: Project-Structure-Analysis.md file]

## 🎯 Specific Areas for Review

### 1. **Architecture & Design Patterns**
- Evaluate my Clean Architecture implementation
- Review CQRS pattern usage with MediatR
- Assess Repository pattern implementation
- Validate Dependency Injection structure
- Check separation of concerns across layers

### 2. **Blazor Architecture & Performance**
- Review Blazor Server vs WebAssembly hybrid approach
- Evaluate component organization and structure
- Assess state management patterns
- Review rendering performance optimizations
- Check SignalR usage and connection management

### 3. **Project Structure & Organization**
- Validate folder structure and naming conventions
- Review project dependencies and references
- Assess feature-based vs layer-based organization
- Check code organization best practices

### 4. **Performance & Scalability**
- Database query optimization strategies
- Caching implementation recommendations
- Memory management in Blazor components
- Async/await patterns usage
- Bundle size optimization for WebAssembly

### 5. **Security & Best Practices**
- Input validation and sanitization
- Authentication/Authorization patterns
- CSRF protection in Blazor
- Secure API design
- Data protection strategies

## 🔍 Please Analyze Against Industry Standards

### **Microsoft Official Recommendations:**
- .NET Application Architecture Guides
- Blazor best practices documentation
- Clean Architecture templates
- Performance optimization guidelines

### **Professional GitHub Projects:**
Please compare with these types of professional .NET projects:
- **eShopOnContainers** (Microsoft's reference architecture)
- **CleanArchitecture** (Jason Taylor's template)
- **Blazor samples** from dotnet organization
- **Enterprise-level Blazor applications**
- **CQRS/Event Sourcing implementations**

### **Industry Expert Opinions:**
Consider recommendations from:
- **Microsoft .NET Team** (official docs and blogs)
- **Jason Taylor** (Clean Architecture creator)
- **Steve Smith** (Ardalis - .NET architecture expert)
- **Jimmy Bogard** (MediatR creator)
- **Blazor community leaders** and MVPs

## 📊 Specific Questions to Address

### **Architecture Questions:**
1. Is my current Clean Architecture implementation following best practices?
2. Should I separate the API layer from Blazor Server project?
3. Is the Shared layer properly designed and utilized?
4. Are my CQRS handlers properly structured?
5. Should I implement Domain Events?

### **Blazor-Specific Questions:**
1. Is the hybrid Server/WebAssembly approach justified for this project?
2. How should I organize components for maximum reusability?
3. What's the best state management approach for this architecture?
4. Should I implement a separate client-side service layer?
5. How can I optimize component rendering performance?

### **Performance Questions:**
1. What caching strategies should I implement?
2. How can I optimize Entity Framework queries?
3. What are the best practices for Blazor component lifecycle management?
4. Should I implement lazy loading for components?
5. How can I minimize bundle size and improve load times?

### **Scalability Questions:**
1. How should I structure the project for team collaboration?
2. What patterns should I use for feature modularity?
3. How can I prepare the architecture for microservices migration?
4. What testing strategies should I implement?
5. How should I handle configuration management across environments?

## 🛠️ Deliverables Requested

### **1. Architectural Assessment:**
- Strengths and weaknesses of current structure
- Compliance with Clean Architecture principles
- CQRS implementation quality
- Dependency flow validation

### **2. Concrete Improvement Plan:**
- Step-by-step refactoring recommendations
- Priority levels (Critical, High, Medium, Low)
- Estimated effort for each improvement
- Risk assessment for changes

### **3. Code Structure Recommendations:**
- Detailed folder structure proposal
- Naming convention standards
- Project organization best practices
- Component hierarchy suggestions

### **4. Performance Optimization Plan:**
- Database optimization strategies
- Blazor performance improvements
- Caching implementation guide
- Memory management recommendations

### **5. Best Practices Implementation:**
- Security hardening checklist
- Error handling patterns
- Logging and monitoring setup
- Testing strategy recommendations

### **6. Technology Stack Validation:**
- Current package versions review
- Alternative technology suggestions
- Integration pattern recommendations
- Third-party library assessments

## 🎯 Expected Output Format

Please provide your analysis in this structure:

### **Executive Summary**
- Overall architecture quality (1-10 scale)
- Top 3 critical issues to address
- Top 3 strengths to maintain

### **Detailed Analysis by Layer**
- Domain Layer assessment
- Application Layer review
- Infrastructure Layer evaluation
- Web Layer (Blazor) analysis
- Shared Layer validation

### **Performance & Scalability Review**
- Current performance bottlenecks
- Scalability limitations
- Optimization opportunities

### **Actionable Recommendations**
- **Immediate Actions** (0-2 weeks)
- **Short-term Improvements** (1-2 months)
- **Long-term Enhancements** (3-6 months)

### **Code Examples**
- Before/After code snippets
- Implementation examples for recommendations
- Configuration samples

### **Reference Materials**
- Links to relevant Microsoft documentation
- GitHub repository examples
- Blog posts and articles from experts
- Community resources and tools

## 🔗 Additional Context

**Project Goals:**
- High-performance note-taking application
- Scalable architecture for future features
- Maintainable codebase for team development
- Modern .NET best practices implementation

**Current Pain Points:**
- Blazor project structure confusion
- Uncertainty about hybrid approach benefits
- Component organization challenges
- Performance optimization needs

**Future Requirements:**
- Real-time collaboration features
- Mobile application support
- Multi-tenant architecture
- Advanced search capabilities
- File attachment support

Please provide a comprehensive, actionable review that will help me build a production-ready, scalable, and maintainable application following industry best practices.
```

## 🎯 Alternative Shorter Prompt

```
I need an expert architectural review of my .NET 10 Clean Architecture + Blazor project. 

**Project:** Note-taking app using Clean Architecture, CQRS (MediatR), EF Core, Blazor Server/WASM hybrid.

**Attached:** Complete project structure analysis with current implementation details.

**Please review against:**
- Microsoft's official .NET architecture guidelines
- Professional GitHub projects (eShopOnContainers, CleanArchitecture template)
- Industry expert recommendations (Jason Taylor, Steve Smith, Jimmy Bogard)
- Blazor best practices and performance patterns

**Focus areas:**
1. Clean Architecture implementation quality
2. Blazor hybrid approach validation
3. CQRS pattern usage assessment
4. Performance optimization opportunities
5. Scalability and maintainability improvements

**Deliverables needed:**
- Architecture quality assessment (1-10 scale)
- Critical issues prioritized list
- Step-by-step improvement plan
- Code structure recommendations
- Performance optimization strategies
- Reference to professional examples

Please provide actionable, specific recommendations with code examples and industry references.
```

## 🎯 Focused Technical Prompt

```
**EXPERT .NET ARCHITECTURE REVIEW REQUEST**

**Project:** Clean Architecture + CQRS + Blazor Server/WASM note-taking application

**Current Stack:** .NET 10, MediatR, EF Core, FluentValidation, Bootstrap 5

**Review Focus:**
1. **Architecture Compliance:** Clean Architecture + CQRS implementation quality
2. **Blazor Strategy:** Server/WASM hybrid approach validation  
3. **Performance:** EF queries, component rendering, state management
4. **Structure:** Project organization, naming, dependencies
5. **Scalability:** Team collaboration, feature modularity, testing

**Compare Against:**
- Microsoft eShopOnContainers
- Jason Taylor's CleanArchitecture template
- Official .NET architecture guides
- Blazor performance best practices

**Deliver:**
- Architecture score (1-10) with justification
- Top 5 critical improvements (prioritized)
- Concrete refactoring steps with code examples
- Performance optimization checklist
- Professional project references

**Attached:** Complete project structure analysis with implementation details.

Please provide expert-level, actionable recommendations based on industry standards and professional project examples.
```

---

## 💡 Tips for Best Results

### **Before Sending:**
1. ✅ Attach the `Project-Structure-Analysis.md` file
2. ✅ Specify your experience level (.NET beginner/intermediate/advanced)
3. ✅ Mention your team size and project timeline
4. ✅ Include any specific constraints (budget, hosting, etc.)

### **Follow-up Questions:**
- Ask for specific GitHub repositories to study
- Request code examples for recommended patterns
- Ask about migration strategies for improvements
- Inquire about testing approaches for the architecture

### **Expected Response Quality:**
- Detailed analysis with scoring/ratings
- Specific code examples and improvements
- Links to professional projects and documentation
- Prioritized action plan with timelines
- Performance benchmarks and optimization strategies

Choose the prompt that best fits your needs - the comprehensive version for detailed analysis or the shorter ones for focused reviews! 🚀