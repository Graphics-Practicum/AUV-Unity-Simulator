# Physics 
This markdown file contains my notes on how to create a realistic physics 
simulation in Unity.  The file is broken down into sections, with each section
focusing on a specific part of the physics. 

## Section 0: Coordinate Systems
To implement Physics consistently, I will be using unity's coordinate system. 

Use the right hand rule to find the axes. Pointer finger points forward, thumb
 points up, and middle points left. 
* the pointer finger points in the positive x direction (forward)
* the thumb points in the positive y direction (up)
* the middle finger points in the positive z direction (left) 

Positive rotation values rotate the object clockwise on a particular axis and 
negative rotation values rotate the object counter-clockwise on a particular 
axis. 

## Section 1: Water Physics
We know that $a_g = 9.81 m/s^2$ for free fall. However, objects in water behave
differently because there are extra forces to consider.

### Buoyancy Force
[source: how to calculate force of buoyancy](https://study.com/skill/learn/how-to-calculate-the-buoyant-force-of-a-floating-object-explanation.html#:~:text=To%20calculate%20the%20buoyant%20force%20we%20can%20use%20the%20equation,the%20acceleration%20due%20to%20gravity.)

Mass of water displaced exerts a force upwards on the object. The equation is 
$F_b=\rho Va_g$ 

1. $\rho$ is the density of the fluid in $kg/m^3$
2. $V$ is the volume of fluid displaced in $m^3$ 
3. $a_g$ is the acceleration due to gravity in $m/s^2$

We assume objects have uniform distribution of material. Ex: Let $A = 1 m^3$ be
the volume of our cube.  If $61\%$ of $A$ is submerged underneath the water 
line, then we will have displaced $A\cdot0.63 = 0.63 m^3$ of water. 

The density of water is $997 kg/m^3$ and $a_g = 9.81 m/s^2$. Now we need to 
calculate the amount of water displaced by the object. We will design an 
algorithm for mesh slicing.

1. define the plane by specifying a **normal vector** and a **point** on the plane
2. 

1. define the slicing plane by specifying a normal vector and a point on the  plane.
2. identify the triangles in the mesh that intersect the slicing pplane
3. Compute the intersection points between the slicing plane and the identified 
edges or triangles.


[source: Unity MeshCollider.sharedMesh](https://docs.unity3d.com/ScriptReference/MeshCollider-sharedMesh.html)
[source: Unity ]

### Drag Forces