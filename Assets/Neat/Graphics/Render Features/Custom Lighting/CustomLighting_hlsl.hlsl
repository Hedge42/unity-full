#ifndef CUSTOM_LIGHTING_INCLUDED
#define CUSTOM_LIGHTING_INCLUDED

struct CustomLightingData
{
    float3 albedo;
};

float3 CalculateCustomLighting(CustomLightingData d)
{
    return d.albedo;
}

void CalculateCustomLighting_float(float3 Albedo, out float3 Color)
{
    CustomLightingData d;
    d.albedo = Albedo;
    
    Color = CalculateCustomLighting(d);
}

#endif