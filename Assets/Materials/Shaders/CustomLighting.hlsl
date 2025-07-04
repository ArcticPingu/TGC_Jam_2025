#ifndef ADDITIONAL_LIGHT_INCLUDED
#define ADDITIONAL_LIGHT_INCLUDED

void MainLight_float(float3 WorldPos, out float3 Direction, out float3 Color, out float Attenuation)
{
#ifdef SHADERGRAPH_PREVIEW
    Direction = normalize(float3(1.0f, 1.0f, 0.0f));
    Color = 1.0f;
    Attenuation = 1.0f;
#else
    Light mainLight = GetMainLight();
    Direction = mainLight.direction;
    Color = mainLight.color;
    Attenuation = mainLight.distanceAttenuation * mainLight.shadowAttenuation;
#endif
}

void MainLight_half(half3 WorldPos, out half3 Direction, out half3 Color, out half Attenuation)
{
#ifdef SHADERGRAPH_PREVIEW
    Direction = normalize(half3(1.0f, 1.0f, 0.0f));
    Color = 1.0f;
    Attenuation = 1.0f;
#else
    Light mainLight = GetMainLight();
    Direction = mainLight.direction;
    Color = mainLight.color;
    Attenuation = mainLight.distanceAttenuation * mainLight.shadowAttenuation;
#endif
}

// This function is problematic in deferred - it will only return one light reliably
void AdditionalLight_float(float3 WorldPos, int lightID, out float3 Direction, out float3 Color, out float Attenuation)
{
    Direction = normalize(float3(1.0f, 1.0f, 0.0f));
    Color = 0.0f;
    Attenuation = 0.0f;
    
#ifndef SHADERGRAPH_PREVIEW
    // In deferred mode, this will be unreliable - use AllAdditionalLights instead
    int lightCount = GetAdditionalLightsCount();
    
    if(lightID >= 0 && lightID < lightCount)
    {
        Light light = GetAdditionalLight(lightID, WorldPos);
        
        if(light.distanceAttenuation > 0.001)
        {
            Direction = light.direction;
            Color = light.color;
            Attenuation = light.distanceAttenuation * light.shadowAttenuation;
        }
    }
#endif
}

void AdditionalLight_half(half3 WorldPos, int lightID, out half3 Direction, out half3 Color, out half Attenuation)
{
    Direction = normalize(half3(1.0f, 1.0f, 0.0f));
    Color = 0.0f;
    Attenuation = 0.0f;
    
#ifndef SHADERGRAPH_PREVIEW
    int lightCount = GetAdditionalLightsCount();
    
    if(lightID >= 0 && lightID < lightCount)
    {
        Light light = GetAdditionalLight(lightID, WorldPos);
        
        if(light.distanceAttenuation > 0.001)
        {
            Direction = light.direction;
            Color = light.color;
            Attenuation = light.distanceAttenuation * light.shadowAttenuation;
        }
    }
#endif
}

// This is the reliable way to get all lights in deferred mode
void AllAdditionalLights_float(float3 WorldPos, float3 WorldNormal, float2 CutoffThresholds, out float3 LightColor)
{
    LightColor = 0.0f;
    
#ifndef SHADERGRAPH_PREVIEW
    // Force a specific number of iterations to avoid dynamic branching issues
    // This is crucial for deferred rendering stability
    for(int i = 0; i < 8; ++i)  // Fixed iteration count
    {
        Light light = GetAdditionalLight(i, WorldPos);
        
        // Check if this light slot is valid
        float lightIntensity = dot(light.color, float3(0.299, 0.587, 0.114));
        
        if(lightIntensity > 0.001 && light.distanceAttenuation > 0.001)
        {
            float NdotL = saturate(dot(light.direction, WorldNormal));
            float lightFactor = smoothstep(CutoffThresholds.x, CutoffThresholds.y, NdotL);
            
            float3 lightContribution = lightFactor * light.color * light.distanceAttenuation * light.shadowAttenuation;
            LightColor += lightContribution;
        }
    }
#endif
}

void AllAdditionalLights_half(half3 WorldPos, half3 WorldNormal, half2 CutoffThresholds, out half3 LightColor)
{
    LightColor = 0.0f;
    
#ifndef SHADERGRAPH_PREVIEW
    // Fixed iteration count for stability
    for(int i = 0; i < 8; ++i)
    {
        Light light = GetAdditionalLight(i, WorldPos);
        
        half lightIntensity = dot(light.color, half3(0.299, 0.587, 0.114));
        
        if(lightIntensity > 0.001 && light.distanceAttenuation > 0.001)
        {
            half NdotL = saturate(dot(light.direction, WorldNormal));
            half lightFactor = smoothstep(CutoffThresholds.x, CutoffThresholds.y, NdotL);
            
            half3 lightContribution = lightFactor * light.color * light.distanceAttenuation * light.shadowAttenuation;
            LightColor += lightContribution;
        }
    }
#endif
}

// Alternative approach: Get the closest/brightest light only
void GetBestAdditionalLight_float(float3 WorldPos, float3 WorldNormal, out float3 Direction, out float3 Color, out float Attenuation)
{
    Direction = normalize(float3(1.0f, 1.0f, 0.0f));
    Color = 0.0f;
    Attenuation = 0.0f;
    
#ifndef SHADERGRAPH_PREVIEW
    float bestIntensity = 0.0f;
    Light bestLight;
    
    // Check all possible light slots
    for(int i = 0; i < 8; ++i)
    {
        Light light = GetAdditionalLight(i, WorldPos);
        
        float lightIntensity = dot(light.color, float3(0.299, 0.587, 0.114)) * light.distanceAttenuation;
        
        if(lightIntensity > bestIntensity && lightIntensity > 0.001)
        {
            bestIntensity = lightIntensity;
            bestLight = light;
        }
    }
    
    if(bestIntensity > 0.001)
    {
        Direction = bestLight.direction;
        Color = bestLight.color;
        Attenuation = bestLight.distanceAttenuation * bestLight.shadowAttenuation;
    }
#endif
}

void GetBestAdditionalLight_half(half3 WorldPos, half3 WorldNormal, out half3 Direction, out half3 Color, out half Attenuation)
{
    Direction = normalize(half3(1.0f, 1.0f, 0.0f));
    Color = 0.0f;
    Attenuation = 0.0f;
    
#ifndef SHADERGRAPH_PREVIEW
    half bestIntensity = 0.0f;
    Light bestLight;
    
    for(int i = 0; i < 8; ++i)
    {
        Light light = GetAdditionalLight(i, WorldPos);
        
        half lightIntensity = dot(light.color, half3(0.299, 0.587, 0.114)) * light.distanceAttenuation;
        
        if(lightIntensity > bestIntensity && lightIntensity > 0.001)
        {
            bestIntensity = lightIntensity;
            bestLight = light;
        }
    }
    
    if(bestIntensity > 0.001)
    {
        Direction = bestLight.direction;
        Color = bestLight.color;
        Attenuation = bestLight.distanceAttenuation * bestLight.shadowAttenuation;
    }
#endif
}

#endif // ADDITIONAL_LIGHT_INCLUDED