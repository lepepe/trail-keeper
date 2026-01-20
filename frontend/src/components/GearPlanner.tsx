'use client';

import { useState } from 'react';
import { api, Gear, GearCreate } from '@/lib/api';

interface GearPlannerProps {
  tripId: number;
  gear: Gear[];
  onUpdate: () => void;
}

export default function GearPlanner({ tripId, gear, onUpdate }: GearPlannerProps) {
  const [name, setName] = useState('');
  const [quantity, setQuantity] = useState(1);
  const [packed, setPacked] = useState(false);
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (!name.trim()) return;

    setLoading(true);
    try {
      await api.addGear(tripId, { name, quantity, packed });
      setName('');
      setQuantity(1);
      setPacked(false);
      onUpdate();
    } catch (error) {
      console.error('Failed to add gear:', error);
    } finally {
      setLoading(false);
    }
  };

  const togglePacked = async (item: Gear) => {
    try {
      await api.updateGear(tripId, item.id, {
        name: item.name,
        quantity: item.quantity,
        packed: !item.packed,
      });
      onUpdate();
    } catch (error) {
      console.error('Failed to update gear:', error);
    }
  };

  const deleteGear = async (gearId: number) => {
    try {
      await api.deleteGear(tripId, gearId);
      onUpdate();
    } catch (error) {
      console.error('Failed to delete gear:', error);
    }
  };

  return (
    <div className="bg-white dark:bg-gray-800 rounded-lg shadow-md dark:shadow-gray-900/50 p-4 border border-transparent dark:border-gray-700">
      <h2 className="text-xl font-semibold mb-4 text-gray-800 dark:text-gray-100">Gear Planner</h2>

      {/* Gear List */}
      {gear.length > 0 ? (
        <div className="mb-4 max-h-48 overflow-y-auto">
          <table className="w-full text-sm">
            <thead>
              <tr className="border-b dark:border-gray-700">
                <th className="text-left py-2 text-gray-700 dark:text-gray-300">Item</th>
                <th className="text-center py-2 text-gray-700 dark:text-gray-300">Qty</th>
                <th className="text-center py-2 text-gray-700 dark:text-gray-300">Packed</th>
                <th></th>
              </tr>
            </thead>
            <tbody>
              {gear.map((item) => (
                <tr key={item.id} className="border-b dark:border-gray-700 last:border-b-0">
                  <td className="py-2 text-gray-800 dark:text-gray-200">{item.name}</td>
                  <td className="py-2 text-center text-gray-600 dark:text-gray-400">{item.quantity}</td>
                  <td className="py-2 text-center">
                    <input
                      type="checkbox"
                      checked={item.packed}
                      onChange={() => togglePacked(item)}
                      className="rounded dark:bg-gray-700 dark:border-gray-600"
                    />
                  </td>
                  <td className="py-2">
                    <button
                      onClick={() => deleteGear(item.id)}
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
        <p className="text-gray-500 dark:text-gray-400 text-sm mb-4">No gear added yet.</p>
      )}

      {/* Add Form */}
      <form onSubmit={handleSubmit} className="space-y-3">
        <div>
          <input
            type="text"
            placeholder="Gear name"
            value={name}
            onChange={(e) => setName(e.target.value)}
            className="w-full px-3 py-2 border dark:border-gray-600 rounded-md text-sm bg-white dark:bg-gray-700 text-gray-800 dark:text-gray-200 placeholder-gray-400 dark:placeholder-gray-500"
          />
        </div>
        <div className="flex gap-2">
          <input
            type="number"
            min="1"
            value={quantity}
            onChange={(e) => setQuantity(Number(e.target.value))}
            className="w-20 px-3 py-2 border dark:border-gray-600 rounded-md text-sm bg-white dark:bg-gray-700 text-gray-800 dark:text-gray-200"
          />
          <label className="flex items-center gap-2 text-sm text-gray-700 dark:text-gray-300">
            <input
              type="checkbox"
              checked={packed}
              onChange={(e) => setPacked(e.target.checked)}
              className="rounded dark:bg-gray-700 dark:border-gray-600"
            />
            Packed
          </label>
        </div>
        <button
          type="submit"
          disabled={loading || !name.trim()}
          className="w-full bg-blue-500 hover:bg-blue-600 dark:bg-blue-600 dark:hover:bg-blue-700 text-white py-2 rounded-md text-sm disabled:opacity-50 transition-colors"
        >
          {loading ? 'Adding...' : 'Add Gear'}
        </button>
      </form>
    </div>
  );
}
