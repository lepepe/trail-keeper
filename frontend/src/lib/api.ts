const API_URL = process.env.NEXT_PUBLIC_API_URL || 'http://localhost:5000';

export enum TripType {
  Kayak = 0,
  Hiking = 1,
  General = 2
}

export interface Trip {
  id: number;
  name: string;
  description?: string;
  type: TripType;
  points?: Point[];
  paths?: Path[];
  gear?: Gear[];
  food?: Food[];
  nights?: Night[];
}

export interface Point {
  id: number;
  tripId: number;
  name: string;
  latitude: number;
  longitude: number;
  icon?: string;
  color?: string;
}

export interface Path {
  id: number;
  tripId: number;
  latitude: number;
  longitude: number;
}

export interface Gear {
  id: number;
  tripId: number;
  name: string;
  quantity: number;
  packed: boolean;
}

export interface Food {
  id: number;
  tripId: number;
  name: string;
  quantity: number;
  eaten: boolean;
}

export interface Night {
  id: number;
  tripId: number;
  nightNumber: number;
  campsite?: string;
  notes?: string;
}

export interface GearCreate {
  name: string;
  quantity: number;
  packed: boolean;
}

export interface FoodCreate {
  name: string;
  quantity: number;
  eaten: boolean;
}

export interface NightCreate {
  nightNumber: number;
  campsite?: string;
  notes?: string;
}

export const api = {
  async getTrips(): Promise<Trip[]> {
    const res = await fetch(`${API_URL}/api/trips`);
    if (!res.ok) throw new Error('Failed to fetch trips');
    return res.json();
  },

  async getTrip(id: number): Promise<Trip> {
    const res = await fetch(`${API_URL}/api/trips/${id}`);
    if (!res.ok) throw new Error('Failed to fetch trip');
    return res.json();
  },

  async addGear(tripId: number, gear: GearCreate): Promise<Gear> {
    const res = await fetch(`${API_URL}/api/trips/${tripId}/gear`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(gear),
    });
    if (!res.ok) throw new Error('Failed to add gear');
    return res.json();
  },

  async updateGear(tripId: number, gearId: number, gear: GearCreate): Promise<void> {
    const res = await fetch(`${API_URL}/api/trips/${tripId}/gear/${gearId}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(gear),
    });
    if (!res.ok) throw new Error('Failed to update gear');
  },

  async deleteGear(tripId: number, gearId: number): Promise<void> {
    const res = await fetch(`${API_URL}/api/trips/${tripId}/gear/${gearId}`, {
      method: 'DELETE',
    });
    if (!res.ok) throw new Error('Failed to delete gear');
  },

  async addFood(tripId: number, food: FoodCreate): Promise<Food> {
    const res = await fetch(`${API_URL}/api/trips/${tripId}/food`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(food),
    });
    if (!res.ok) throw new Error('Failed to add food');
    return res.json();
  },

  async updateFood(tripId: number, foodId: number, food: FoodCreate): Promise<void> {
    const res = await fetch(`${API_URL}/api/trips/${tripId}/food/${foodId}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(food),
    });
    if (!res.ok) throw new Error('Failed to update food');
  },

  async deleteFood(tripId: number, foodId: number): Promise<void> {
    const res = await fetch(`${API_URL}/api/trips/${tripId}/food/${foodId}`, {
      method: 'DELETE',
    });
    if (!res.ok) throw new Error('Failed to delete food');
  },

  async addNight(tripId: number, night: NightCreate): Promise<Night> {
    const res = await fetch(`${API_URL}/api/trips/${tripId}/nights`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(night),
    });
    if (!res.ok) throw new Error('Failed to add night');
    return res.json();
  },

  async updateNight(tripId: number, nightId: number, night: NightCreate): Promise<void> {
    const res = await fetch(`${API_URL}/api/trips/${tripId}/nights/${nightId}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(night),
    });
    if (!res.ok) throw new Error('Failed to update night');
  },

  async deleteNight(tripId: number, nightId: number): Promise<void> {
    const res = await fetch(`${API_URL}/api/trips/${tripId}/nights/${nightId}`, {
      method: 'DELETE',
    });
    if (!res.ok) throw new Error('Failed to delete night');
  },
};
