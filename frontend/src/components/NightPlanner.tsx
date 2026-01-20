'use client';

import { useState } from 'react';
import { api, Night, NightCreate } from '@/lib/api';

interface NightPlannerProps {
  tripId: number;
  nights: Night[];
  onUpdate: () => void;
}

export default function NightPlanner({ tripId, nights, onUpdate }: NightPlannerProps) {
  const [nightNumber, setNightNumber] = useState(1);
  const [campsite, setCampsite] = useState('');
  const [notes, setNotes] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    setLoading(true);
    try {
      await api.addNight(tripId, { nightNumber, campsite: campsite || undefined, notes: notes || undefined });
      setNightNumber((nights.length > 0 ? Math.max(...nights.map(n => n.nightNumber)) : 0) + 1);
      setCampsite('');
      setNotes('');
      onUpdate();
    } catch (error) {
      console.error('Failed to add night:', error);
    } finally {
      setLoading(false);
    }
  };

  const deleteNight = async (nightId: number) => {
    try {
      await api.deleteNight(tripId, nightId);
      onUpdate();
    } catch (error) {
      console.error('Failed to delete night:', error);
    }
  };

  return (
    <div className="bg-white dark:bg-gray-800 rounded-lg shadow-md dark:shadow-gray-900/50 p-4 border border-transparent dark:border-gray-700">
      <h2 className="text-xl font-semibold mb-4 text-gray-800 dark:text-gray-100">Night Planner</h2>

      {/* Nights List */}
      {nights.length > 0 ? (
        <div className="mb-4 max-h-48 overflow-y-auto">
          <table className="w-full text-sm">
            <thead>
              <tr className="border-b dark:border-gray-700">
                <th className="text-left py-2 text-gray-700 dark:text-gray-300">Night</th>
                <th className="text-left py-2 text-gray-700 dark:text-gray-300">Campsite</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {nights.sort((a, b) => a.nightNumber - b.nightNumber).map((night) => (
                <tr key={night.id} className="border-b dark:border-gray-700 last:border-b-0">
                  <td className="py-2 text-gray-800 dark:text-gray-200">{night.nightNumber}</td>
                  <td className="py-2">
                    <div className="text-gray-800 dark:text-gray-200">{night.campsite || '-'}</div>
                    {night.notes && (
                      <div className="text-xs text-gray-500 dark:text-gray-400">{night.notes}</div>
                    )}
                  </td>
                  <td className="py-2">
                    <button
                      onClick={() => deleteNight(night.id)}
                      className="text-red-500 hover:text-red-700 dark:text-red-400 dark:hover:text-red-300 text-xs"
                    >
                      Delete
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      ) : (
        <p className="text-gray-500 dark:text-gray-400 text-sm mb-4">No nights planned yet.</p>
      )}

      {/* Add Form */}
      <form onSubmit={handleSubmit} className="space-y-3">
        <div className="flex gap-2">
          <div className="w-20">
            <label className="block text-xs text-gray-500 dark:text-gray-400 mb-1">Night #</label>
            <input
              type="number"
              min="1"
              value={nightNumber}
              onChange={(e) => setNightNumber(Number(e.target.value))}
              className="w-full px-3 py-2 border dark:border-gray-600 rounded-md text-sm bg-white dark:bg-gray-700 text-gray-800 dark:text-gray-200"
            />
          </div>
          <div className="flex-1">
            <label className="block text-xs text-gray-500 dark:text-gray-400 mb-1">Campsite</label>
            <input
              type="text"
              placeholder="Campsite name"
              value={campsite}
              onChange={(e) => setCampsite(e.target.value)}
              className="w-full px-3 py-2 border dark:border-gray-600 rounded-md text-sm bg-white dark:bg-gray-700 text-gray-800 dark:text-gray-200 placeholder-gray-400 dark:placeholder-gray-500"
            />
          </div>
        </div>
        <div>
          <label className="block text-xs text-gray-500 dark:text-gray-400 mb-1">Notes</label>
          <textarea
            placeholder="Notes for this night..."
            value={notes}
            onChange={(e) => setNotes(e.target.value)}
            className="w-full px-3 py-2 border dark:border-gray-600 rounded-md text-sm resize-none bg-white dark:bg-gray-700 text-gray-800 dark:text-gray-200 placeholder-gray-400 dark:placeholder-gray-500"
            rows={2}
          />
        </div>
        <button
          type="submit"
          disabled={loading}
          className="w-full bg-purple-500 hover:bg-purple-600 dark:bg-purple-600 dark:hover:bg-purple-700 text-white py-2 rounded-md text-sm disabled:opacity-50 transition-colors"
        >
          {loading ? 'Adding...' : 'Add Night'}
        </button>
      </form>
    </div>
  );
}
