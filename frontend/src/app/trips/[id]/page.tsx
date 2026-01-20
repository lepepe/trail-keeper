'use client';

import { useEffect, useState, useCallback } from 'react';
import { useParams } from 'next/navigation';
import Link from 'next/link';
import dynamic from 'next/dynamic';
import { api, Trip } from '@/lib/api';
import GearPlanner from '@/components/GearPlanner';
import FoodPlanner from '@/components/FoodPlanner';
import NightPlanner from '@/components/NightPlanner';
import ThemeToggle from '@/components/ThemeToggle';

const TripMap = dynamic(() => import('@/components/TripMap'), { ssr: false });

export default function TripDetail() {
  const params = useParams();
  const tripId = Number(params.id);
  const [trip, setTrip] = useState<Trip | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const loadTrip = useCallback(async () => {
    try {
      const data = await api.getTrip(tripId);
      setTrip(data);
    } catch (err) {
      setError('Failed to load trip details.');
    } finally {
      setLoading(false);
    }
  }, [tripId]);

  useEffect(() => {
    loadTrip();
  }, [loadTrip]);

  if (loading) {
    return (
      <div className="flex justify-center items-center h-screen bg-gray-100 dark:bg-gray-900">
        <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500"></div>
      </div>
    );
  }

  if (error || !trip) {
    return (
      <div className="container mx-auto px-4 py-8">
        <div className="bg-red-100 dark:bg-red-900/30 border border-red-400 dark:border-red-800 text-red-700 dark:text-red-400 px-4 py-3 rounded">
          {error || 'Trip not found'}
        </div>
        <Link href="/" className="text-blue-600 dark:text-blue-400 hover:underline mt-4 inline-block">
          &larr; Back to trips
        </Link>
      </div>
    );
  }

  return (
    <main className="min-h-screen bg-gray-100 dark:bg-gray-900">
      <header className="bg-white dark:bg-gray-800 shadow dark:shadow-gray-900/50 border-b border-transparent dark:border-gray-700">
        <div className="container mx-auto px-4 py-4">
          <div className="flex items-center justify-between">
            <Link href="/" className="text-blue-600 dark:text-blue-400 hover:underline text-sm">
              &larr; Back to trips
            </Link>
            <ThemeToggle />
          </div>
          <h1 className="text-3xl font-bold text-gray-800 dark:text-gray-100 mt-2">{trip.name}</h1>
          {trip.description && (
            <p className="text-gray-600 dark:text-gray-400 mt-1">{trip.description}</p>
          )}
        </div>
      </header>

      <div className="container mx-auto px-4 py-6">
        <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
          {/* Map Section */}
          <div className="lg:col-span-2">
            <div className="bg-white dark:bg-gray-800 rounded-lg shadow-md dark:shadow-gray-900/50 overflow-hidden border border-transparent dark:border-gray-700">
              <div className="p-4 border-b dark:border-gray-700">
                <h2 className="text-xl font-semibold text-gray-800 dark:text-gray-100">Route Map</h2>
              </div>
              <div className="h-[500px]">
                <TripMap
                  points={trip.points || []}
                  paths={trip.paths || []}
                />
              </div>
            </div>

            {/* Waypoints Summary */}
            {trip.points && trip.points.length > 0 && (
              <div className="bg-white dark:bg-gray-800 rounded-lg shadow-md dark:shadow-gray-900/50 mt-6 p-4 border border-transparent dark:border-gray-700">
                <h2 className="text-xl font-semibold mb-4 text-gray-800 dark:text-gray-100">Waypoints</h2>
                <div className="overflow-x-auto">
                  <table className="w-full text-sm">
                    <thead>
                      <tr className="border-b dark:border-gray-700">
                        <th className="text-left py-2 text-gray-700 dark:text-gray-300">Name</th>
                        <th className="text-left py-2 text-gray-700 dark:text-gray-300">Latitude</th>
                        <th className="text-left py-2 text-gray-700 dark:text-gray-300">Longitude</th>
                      </tr>
                    </thead>
                    <tbody>
                      {trip.points.map((point) => (
                        <tr key={point.id} className="border-b dark:border-gray-700 last:border-b-0">
                          <td className="py-2 text-gray-800 dark:text-gray-200">{point.name}</td>
                          <td className="py-2 text-gray-600 dark:text-gray-400">{point.latitude.toFixed(4)}</td>
                          <td className="py-2 text-gray-600 dark:text-gray-400">{point.longitude.toFixed(4)}</td>
                        </tr>
                      ))}
                    </tbody>
                  </table>
                </div>
              </div>
            )}
          </div>

          {/* Planners Section */}
          <div className="space-y-6">
            <GearPlanner
              tripId={tripId}
              gear={trip.gear || []}
              onUpdate={loadTrip}
            />
            <FoodPlanner
              tripId={tripId}
              food={trip.food || []}
              onUpdate={loadTrip}
            />
            <NightPlanner
              tripId={tripId}
              nights={trip.nights || []}
              onUpdate={loadTrip}
            />
          </div>
        </div>
      </div>

      <footer className="bg-white dark:bg-gray-800 border-t dark:border-gray-700 mt-8 py-4">
        <div className="container mx-auto px-4 text-center text-gray-500 dark:text-gray-400 text-sm">
          Data source: Suwannee River Water Management District, Florida DEP, USGS
        </div>
      </footer>
    </main>
  );
}
