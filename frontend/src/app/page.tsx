'use client';

import { useEffect, useState } from 'react';
import Link from 'next/link';
import { api, Trip, TripType } from '@/lib/api';
import ThemeToggle from '@/components/ThemeToggle';

export default function Home() {
  const [trips, setTrips] = useState<Trip[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    async function loadTrips() {
      try {
        const data = await api.getTrips();
        setTrips(data);
      } catch (err) {
        setError('Failed to load trips. Make sure the backend is running.');
      } finally {
        setLoading(false);
      }
    }
    loadTrips();
  }, []);

  const getTripTypeIcon = (type: TripType) => {
    switch (type) {
      case TripType.Kayak:
        return '🚣';
      case TripType.Hiking:
        return '🥾';
      default:
        return '🏕️';
    }
  };

  return (
    <main className="container mx-auto px-4 py-8">
      <header className="mb-8 flex items-center justify-between">
        <div>
          <h1 className="text-4xl font-bold text-blue-600 dark:text-blue-400">Water Trail</h1>
          <p className="text-gray-600 dark:text-gray-400 mt-2">Plan and track your outdoor adventures</p>
        </div>
        <ThemeToggle />
      </header>

      {loading && (
        <div className="flex justify-center items-center h-64">
          <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500"></div>
        </div>
      )}

      {error && (
        <div className="bg-red-100 dark:bg-red-900/30 border border-red-400 dark:border-red-800 text-red-700 dark:text-red-400 px-4 py-3 rounded">
          {error}
        </div>
      )}

      {!loading && !error && trips.length === 0 && (
        <div className="text-center py-12">
          <p className="text-gray-500 dark:text-gray-400 text-lg">No trips found. Create your first trip!</p>
        </div>
      )}

      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {trips.map((trip) => (
          <Link href={`/trips/${trip.id}`} key={trip.id}>
            <div className="bg-white dark:bg-gray-800 rounded-lg shadow-md hover:shadow-lg dark:shadow-gray-900/50 transition-shadow p-6 cursor-pointer border border-transparent dark:border-gray-700">
              <div className="flex items-center gap-3 mb-3">
                <span className="text-3xl">{getTripTypeIcon(trip.type)}</span>
                <h2 className="text-xl font-semibold text-gray-800 dark:text-gray-100">{trip.name}</h2>
              </div>
              {trip.description && (
                <p className="text-gray-600 dark:text-gray-400 line-clamp-2">{trip.description}</p>
              )}
              <div className="mt-4 flex items-center text-sm text-blue-600 dark:text-blue-400">
                <span>View Details</span>
                <svg
                  className="w-4 h-4 ml-1"
                  fill="none"
                  stroke="currentColor"
                  viewBox="0 0 24 24"
                >
                  <path
                    strokeLinecap="round"
                    strokeLinejoin="round"
                    strokeWidth={2}
                    d="M9 5l7 7-7 7"
                  />
                </svg>
              </div>
            </div>
          </Link>
        ))}
      </div>
    </main>
  );
}
