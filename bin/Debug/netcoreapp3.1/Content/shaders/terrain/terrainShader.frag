#version 330 core
struct Material {
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
    float shininess;

    float lerpTreshold;
}; 
struct DirectionalLight {
    vec3 direction;
  
    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};  

struct PointLight {    
    vec3 position;
    
    float constant;
    float linear;
    float quadratic;  

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};  

struct SpotLight{
    vec3 position;
    vec3 direction;
    float cutOff;
    float outerCutOff;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;

    float constant;
    float linear;
    float quadratic;
};
#define NR_MATERIALS 6  
uniform Material materials[NR_MATERIALS];
Material material;

uniform DirectionalLight dirLight;
#define NR_POINT_LIGHTS 4  
uniform PointLight pointLights[NR_POINT_LIGHTS];
uniform SpotLight spotLight;

uniform vec3 viewPos;

//fog vars
uniform vec4 skyColor;
in float geomDistance;
float density = 0.0015;
float gradient = 2;

in vec3 geomNormal;
in vec3 geomFragPos;
in vec2 geomTexCoords;
in float geomModifiedY;

out vec4 FragColor;

// prototypes
vec3 CalcDirLight(DirectionalLight light, vec3 geomNormal, vec3 viewDir);  
vec3 CalcPointLight(PointLight light, vec3 geomNormal, vec3 geomFragPos, vec3 viewDir);  
vec3 CalcSpotLight(SpotLight light, vec3 geomNormal, vec3 geomFragPos, vec3 viewDir);
float CalcFogDensity();
Material MixMaterials(Material[NR_MATERIALS] materials);

void main()
{
    // properties
    material = MixMaterials(materials);
    vec3 norm = normalize(geomNormal);
    vec3 viewDir = normalize(viewPos - geomFragPos);

    // phase 1: Directional lighting
    vec3 result = CalcDirLight(dirLight, norm, viewDir);
    // phase 2: Point lights
    for(int i = 0; i < NR_POINT_LIGHTS; i++)
    result += CalcPointLight(pointLights[i], norm, geomFragPos, viewDir);    
    // phase 3: Spot light
    result += CalcSpotLight(spotLight, norm, geomFragPos, viewDir);    

    // phase 4: fog
    result = mix(result, vec3(skyColor.x, skyColor.y, skyColor.z), CalcFogDensity());
    
    FragColor = vec4(result, 1.0);
}

float CalcFogDensity()
{
    float FogDensity = pow((geomDistance * density), gradient);
    FogDensity = clamp(FogDensity, 0, 1);
    return FogDensity;
}

Material MixMaterials(Material[NR_MATERIALS] materials)
{
    float height = clamp((geomModifiedY) / 10, -1, 1);
    height+=1;
    Material x = materials[0];
    float a = 0;
    for(int i = 0; i <  NR_MATERIALS; i++)
    {
        Material y = materials[i];

        x.ambient = mix(x.ambient, y.ambient, smoothstep(x.lerpTreshold, y.lerpTreshold, height));
        x.diffuse = mix(x.diffuse, y.diffuse, smoothstep(x.lerpTreshold, y.lerpTreshold, height));
        x.specular = mix(x.specular, y.ambient, smoothstep(x.lerpTreshold, y.lerpTreshold, height));
        x.shininess = mix(x.shininess, y.shininess, smoothstep(x.lerpTreshold, y.lerpTreshold, height));
    }
    return x;
}


vec3 CalcDirLight(DirectionalLight light, vec3 geomNormal, vec3 viewDir)
{
    vec3 lightDir = normalize(-light.direction);
    // diffuse shading
    float diff = max(dot(geomNormal, lightDir), 0.0);
    // specular shading
    vec3 reflectDir = reflect(-lightDir, geomNormal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    // combine results
    vec3 ambient  = light.ambient  * material.ambient;
    vec3 diffuse  = light.diffuse  * diff * material.diffuse;
    vec3 specular = light.specular * spec * material.specular;
    return (ambient + diffuse + specular);
}  

vec3 CalcPointLight(PointLight light, vec3 geomNormal, vec3 geomFragPos, vec3 viewDir)
{
    vec3 lightDir = normalize(light.position - geomFragPos);
    // diffuse shading
    float diff = max(dot(geomNormal, lightDir), 0.0);
    // specular shading
    vec3 reflectDir = reflect(-lightDir, geomNormal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
    // attenuation
    float distance    = length(light.position - geomFragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance + 
  			     light.quadratic * (distance * distance));    
    // combine results
    vec3 ambient  = light.ambient  * material.ambient;
    vec3 diffuse  = light.diffuse  * diff * material.diffuse;
    vec3 specular = light.specular * spec * material.specular;
    ambient  *= attenuation;
    diffuse  *= attenuation;
    specular *= attenuation;
    return (ambient + diffuse + specular);
} 

vec3 CalcSpotLight(SpotLight light, vec3 geomNormal, vec3 geomFragPos, vec3 viewDir)
{
    //diffuse shading
    vec3 lightDir = normalize(light.position - geomFragPos);
    float diff = max(dot(geomNormal, lightDir), 0.0);

    //specular shading
    vec3 reflectDir = reflect(-lightDir, geomNormal);
    float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);

    //attenuation
    float distance    = length(light.position - geomFragPos);
    float attenuation = 1.0 / (light.constant + light.linear * distance +
    light.quadratic * (distance * distance));

    //spotlight intensity
    float theta     = dot(lightDir, normalize(-light.direction));
    float epsilon   = light.cutOff - light.outerCutOff;
    float intensity = clamp((theta - light.outerCutOff) / epsilon, 0.0, 1.0);

    //combine results
    vec3 ambient = light.ambient * material.ambient;
    vec3 diffuse = light.diffuse * diff * material.diffuse;
    vec3 specular = light.specular * spec * material.specular;;
    ambient  *= attenuation;
    diffuse  *= attenuation * intensity;
    specular *= attenuation * intensity;
    return (ambient + diffuse + specular);
}
